//------------------------------------------------------------------------------------------------------------------------------------
// Copyright 2021 Dassault Systèmes - CPE EMED
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

using System;
using System.IO;
using System.ServiceProcess;
using System.Configuration;

using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace ds.enovia.eif.client
{
    public partial class EIFClientWinService : ServiceBase
    {
        ISession m_session = null; 
        IConnection m_connection = null;
        IMessageConsumer m_consumer = null;
        
        public EIFClientWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            String jmsUrl  = ConfigurationManager.AppSettings["JMS-URL"];
            String agentId = ConfigurationManager.AppSettings["AGENT-ID"];
            String agentCredentials = ConfigurationManager.AppSettings["AGENT-CREDENTIALS"];
            String tenantId = ConfigurationManager.AppSettings["TENANT-ID"];

            String userTopicName = "3dsevents." + tenantId + ".3DSpace.user"; //user "event space"
            String consumerName = tenantId + "-" + agentId;

            //Create the Connection factory 
            IConnectionFactory factory = new ConnectionFactory(jmsUrl);
            
            //Create the connection 
            m_connection = factory.CreateConnection(agentId, agentCredentials);

            m_connection.ClientId = consumerName; //optional
            m_connection.Start();

            //Create the Session 
            m_session = m_connection.CreateSession();
            //Create the Consumer 
            m_consumer = m_session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(userTopicName), consumerName, null, false);
            m_consumer.Listener += new MessageListener(this.consumer_Listener);
        }

        void consumer_Listener(IMessage message)
        {
            //write to file
            AppendToFile(((ITextMessage)message).Text);
        }

        protected override void OnStop()
        {
            m_consumer.Dispose();
            m_session.Dispose();
            m_connection.Dispose();

            m_consumer = null;
            m_session = null;
            m_connection = null;
        }

        void AppendToFile(string message)
        {
            String fileName = ConfigurationManager.AppSettings["OUTPUT-FILENAME"];

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.WriteLine(message);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.WriteLine(message);
                    }
                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}
