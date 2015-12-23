using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQNet
{
    class Program
    {
        static IConnectionFactory _factory = null;
        static IConnection _connection = null;
        static ITextMessage _message = null;

        static void Main(string[] args)
        {
            //创建工厂
            _factory = new ConnectionFactory("tcp://192.168.40.160:61616/");

            try
            {
                //创建连接
                using (_connection = _factory.CreateConnection())
                {
                    //创建会话
                    using (ISession session = _connection.CreateSession())
                    {
                        //创建一个主题
                        IDestination destination = new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("topic");

                        //创建生产者
                        IMessageProducer producer = session.CreateProducer(destination);

                        Console.WriteLine("Please enter any key to continue! ");
                        Console.ReadKey();
                        Console.WriteLine("Sending: ");

                        //创建一个文本消息
                        _message = producer.CreateTextMessage("Hello AcitveMQ....");

                        //发送消息
                        producer.Send(_message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
                        int i = 0;
                        while (true)
                        {
                            i++;
                            //  var msg = Console.ReadLine();
                            var msg = i.ToString();
                            _message = producer.CreateTextMessage(msg);
                            producer.Send(_message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
                        }
                       
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();

        }
    }
}

