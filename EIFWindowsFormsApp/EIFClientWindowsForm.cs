//------------------------------------------------------------------------------------------------------------------------------------
// Copyright 2022 Dassault Systèmes - CPE EMED
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//------------------------------------------------------------------------------------------------------------------------------------

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;

using System;
using System.ComponentModel;
using System.Configuration;

using System.Windows.Forms;

namespace EIFWindowsFormsApp
{
   public partial class EIFClientWindowsForm : Form
   {
      static IConnection m_connection = null;
      static ISession m_session = null;
      static IMessageConsumer m_consumer = null;
      static IConnectionFactory m_factory = null;

      public EIFClientWindowsForm()
      {
         InitializeComponent();
      }

      private void m_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         BackgroundWorker worker = (BackgroundWorker) sender;

         string jmsUrl           = ConfigurationManager.AppSettings["JMS-URL"];
         string agentId          = ConfigurationManager.AppSettings["AGENT-ID"];
         string agentCredentials = ConfigurationManager.AppSettings["AGENT-CREDENTIALS"];
         string tenantId         = ConfigurationManager.AppSettings["TENANT-ID"];

         string userTopicName    = "3dsevents." + tenantId + ".3DSpace.user"; //user "event space"
         string consumerName     = tenantId + "-" + agentId;

         //Create the Connection factory 
         m_factory = new ConnectionFactory(jmsUrl);

         //Create the connection 
         m_connection = m_factory.CreateConnection(agentId, agentCredentials);
         
         m_connection.ClientId = consumerName;

         m_connection.Start();

         //Create the Session 
         m_session = m_connection.CreateSession();
            
         //Create the Consumer 
         m_consumer = m_session.CreateDurableConsumer(new ActiveMQTopic(userTopicName), consumerName, null, false);
         m_consumer.Listener += new MessageListener(consumer_Listener);

         BeginInvoke(new Action(() => { this.m_stopButton.Enabled = true; }));

         while (true)
         {
            if (worker.CancellationPending == true)
            {
               e.Cancel = true;
               break;
            }
         }
      }

      private void consumer_Listener(IMessage message)
      {
         BeginInvoke(new Action(() => { this.m_textBox.AppendText(((ITextMessage)message).Text); }));
      }

      private void m_startButton_Click(object sender, EventArgs e)
      {
         m_startButton.Enabled = false;
         m_backgroundWorker.RunWorkerAsync();
      }

      private void m_stopButton_Click(object sender, EventArgs e)
      {
         m_stopButton.Enabled = false;
         m_backgroundWorker.CancelAsync();
      }

      private async void m_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         m_consumer.Close();
         m_session.Close();

         m_consumer.Dispose();
         m_session.Dispose();

         m_session = null;
         m_consumer = null;

         if (m_connection.IsStarted)
         {
            m_connection.Stop();
         }

         await m_connection.CloseAsync();

         m_connection.Dispose();

         BeginInvoke(new Action(() => { this.m_startButton.Enabled = true; }));
      }
   }
}
