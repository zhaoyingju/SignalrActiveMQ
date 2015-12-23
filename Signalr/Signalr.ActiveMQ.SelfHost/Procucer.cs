using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signalr.ActiveMQ.SelfHost
{
    class Procucer
    {
        IConnectionFactory _factory = null;
        IConnection _connection = null;
        ITextMessage _message = null;

        private  IMessageProducer producer = null;
        private static Procucer instance=null;
        private Procucer()
        {
            instance = this;
            //创建工厂
            _factory = new ConnectionFactory("tcp://192.168.40.160:61616/");

            try
            {
                //创建连接
                _connection = _factory.CreateConnection();
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

        public static Procucer GetInstance()
        {
            if (instance == null)
                instance = new Procucer();
            return instance;
        }

        public void Send(string msg)
        {
            //创建一个文本消息
            _message = producer.CreateTextMessage(msg);
            //发送消息
            producer.Send(_message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
        }
    }
}


