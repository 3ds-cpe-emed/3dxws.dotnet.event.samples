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
using System.Configuration;

using Apache.NMS;
using Apache.NMS.ActiveMQ;

using Microsoft.Azure.WebJobs;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace ds.enovia.eif.client
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    //[StorageAccount("AzureWebJobsStorage")]
    class Program
    {
        static IConnection m_connection = null;
        static ISession m_session = null;
        static IMessageConsumer m_consumer = null;

        static CloudQueue m_queue = null;
        static CloudQueueClient m_queueClient = null;

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var cancellationToken = new WebJobsShutdownWatcher().Token;
            cancellationToken.Register(() =>
            {
                //Console.Out.WriteLine("Do whatever you want before the webjob is stopped...");
                if (m_consumer != null)
                {
                    m_consumer.Close();
                    m_consumer.Dispose();
                }
                //first the session
                if (m_session != null)
                {
                    m_session.Close();
                    m_session.Dispose();
                }
                //next the connection
                if (m_connection != null)
                {
                    m_connection.Close();
                    m_connection.Dispose();
                }

                m_queue = null;
                m_queueClient = null;

            });

            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            Initialize();

            var host = new JobHost(config);

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }

        //[NoAutomaticTrigger]
        public static void Initialize()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString; //"DefaultEndpointsProtocol=https;AccountName=azfundsemedcpestorage;AccountKey=sAtrD7RLfZeYhvOKytN/OxlWnct0HSQnWZN2F4jdd39hV1nEDdW32h9qGimHdWPtYKAWOmTOUgHDS8NXIaiPqw==;EndpointSuffix=core.windows.net";// ConfigurationManager.AppSettings["AzureWebJobsStorage"];

            string eifJMSUrl = ConfigurationManager.AppSettings["EIF-JMS-URL"];
            string agent_id = ConfigurationManager.AppSettings["AGENT-ID"];
            string agent_credentials = ConfigurationManager.AppSettings["AGENT-CREDENTIALS"];
            string tenant_id = ConfigurationManager.AppSettings["TENANT-ID"];
            string queue_name = ConfigurationManager.AppSettings["QUEUE-NAME"];

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            
            //CloudQueueClient 
            m_queueClient = storageAccount.CreateCloudQueueClient();
            m_queue = m_queueClient.GetQueueReference(queue_name);

            String topicname_user = "3dsevents." + tenant_id + ".3DSpace.user";
            String consumer_name = tenant_id + "-" + agent_id;

            //Create the Connection factory 
            IConnectionFactory factory = new ConnectionFactory(eifJMSUrl);

            //Create the connection 
            m_connection = factory.CreateConnection(agent_id, agent_credentials);

            m_connection.ClientId = consumer_name;
            m_connection.Start();

            //Create the Session 
            m_session = m_connection.CreateSession();
                
            //Create the Consumer 
            m_consumer = m_session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(topicname_user), consumer_name, null, false);
            m_consumer.Listener += new MessageListener(consumer_Listener);
        }

        [NoAutomaticTrigger]
        static void consumer_Listener(IMessage message)
        {
            CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(((ITextMessage)message).Text);

            m_queue.AddMessage(cloudQueueMessage);
        }   
    }
}
