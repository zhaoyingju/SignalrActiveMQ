using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SignalR.ActiveMQ
{
    public class Customer
    {
        private static object lockObj = new object();
        private static IHubCallerConnectionContext<dynamic> _clients;
        public static IHubCallerConnectionContext<dynamic> Clients
        {
            get { return _clients; }
            set
            {
                lock (lockObj)
                {
                    _clients = value;
                }
            }
        }
        public static void Run(string cutomerId="",string address= "tcp://127.0.0.1:61616/")
        {
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                try
                {
                    //创建连接工厂
                    IConnectionFactory _factory = new ConnectionFactory(address);
                    //创建连接
                    using (IConnection conn = _factory.CreateConnection())
                    {
                        //设置客户端ID
                        conn.ClientId = cutomerId;
                        conn.Start();
                        //创建会话
                        using (ISession session = conn.CreateSession())
                        {
                            //创建主题
                            var topic = new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("topic");

                            //创建消费者
                            IMessageConsumer consumer = session.CreateDurableConsumer(topic, "Customer", null, false);

                            //注册监听事件
                            consumer.Listener += new MessageListener(consumer_Listener);

                            //阻塞当前线程，监听消息
                            System.Threading.Thread.CurrentThread.Join();
                        }
                        //关闭连接
                        conn.Stop();
                        conn.Close();
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Console.WriteLine(ex.ToString());
                }
               
            });

            t.Start();
        }
        static void consumer_Listener(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            if (Clients != null)
            {
                Clients.All.broadcastMessage(msg.Text);
            }
            Debug.WriteLine("Receive: " + msg.Text);
            Console.WriteLine("Receive: " + msg.Text);
        }
    }
}
