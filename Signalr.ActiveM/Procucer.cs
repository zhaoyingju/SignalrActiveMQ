using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signalr.ActiveMQ
{
  public  class Procucer
    {
        private IMessageProducer producer;
        private static Procucer instance=null;
        private Procucer(string customerId,string address)
        {
            instance = this;
            //创建工厂
            IConnectionFactory _factory = new ConnectionFactory("tcp://127.0.0.1:61616/");

            try
            {
                //创建连接
                IConnection _connection = _factory.CreateConnection();
                {
                    //创建会话
                    ISession session = _connection.CreateSession();
                    {
                        //创建一个主题
                        IDestination destination = new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("topic");

                        //创建生产者
                        producer = session.CreateProducer(destination);

                        Console.WriteLine("Please enter any key to continue! ");
                      //  Console.ReadKey();
                        Console.WriteLine("Sending: ");                      

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //Console.ReadLine();
        }

        public static Procucer GetInstance(string customerId="",string address= "tcp://127.0.0.1:61616/")
        {
            if (instance == null)
                instance = new Procucer(customerId, address);
            return instance;
        }

        public void Send(string msg)
        {
            //创建一个文本消息
            ITextMessage _message = producer.CreateTextMessage(msg);
            //发送消息
            producer.Send(_message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
        }
    }
}


