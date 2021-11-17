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

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Configuration;

// adapted from
// https://www.developer.com/net/article.php/10916_3823691_2/Getting-started-with-Apache-NMS-Framework-and-Apache-ActiveMQ.htm
//
namespace ds.enovia.eif.client
{
    class Program
    {
        static void Main(string[] args)
        {
            String jmsUrl   = ConfigurationManager.AppSettings["JMS-URL"];
            String agentId = ConfigurationManager.AppSettings["AGENT-ID"];
            String agentCredentials = ConfigurationManager.AppSettings["AGENT-CREDENTIALS"];
            String tenantId = ConfigurationManager.AppSettings["TENANT-ID"];

            String userTopicName = "3dsevents." + tenantId + ".3DSpace.user"; //user "event space"
            String consumerName = tenantId + "-" + agentId;

            //Create the Connection factory 
            IConnectionFactory factory = new ConnectionFactory(jmsUrl);
            
            //Create the connection 
            using (IConnection connection = factory.CreateConnection(agentId, agentCredentials))
            {
                connection.ClientId = consumerName; //optional
                connection.Start();

                //Create the Session 
                using (ISession session = connection.CreateSession())
                {
                    //Create the Consumer 
                    IMessageConsumer consumer = session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(userTopicName), consumerName, null, false);
                    consumer.Listener += new MessageListener(consumer_Listener);

                    Console.ReadLine();
                }
            }
        }

        static void consumer_Listener(IMessage message)
        {
            Console.WriteLine("Receive: " + ((ITextMessage)message).Text);
        }
    }
}