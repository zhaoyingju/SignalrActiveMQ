using System;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using IConnection = Apache.NMS.IConnection;

namespace SignalR.ActiveMQ
{
    internal class ActiveMqMessageBus : IMessageBus
    {
        private const string TopicName = "AmqMessageBus";

        private readonly ActiveMqConnectionConfiguration _configuration;
        private readonly MessageBus _bus;

        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private ISession _session;
        private IMessageProducer _producer;
        private IMessageConsumer _consumer;

        public ActiveMqMessageBus(IDependencyResolver resolver, ActiveMqConnectionConfiguration configuration)
        {
            _configuration = configuration;
            _connectionFactory = new ConnectionFactory(configuration.ConnectionString);
            _bus = new MessageBus(resolver);

            EnsureConnection();
        }

        public Task Publish(Message message)
        {
            _producer.Send(ActiveMqMessage.ToMessage(message, _configuration));
            return TaskAsyncHelper.Empty;
        }

        public IDisposable Subscribe(ISubscriber subscriber, string cursor, Func<MessageResult, object, Task<bool>> callback, int maxMessages, object state)
        {
            return _bus.Subscribe(subscriber, cursor, callback, maxMessages, state);
        }

        private void EnsureConnection()
        {
            _connection = _connectionFactory.CreateConnection();
            _session = _connection.CreateSession();
            var topic = _session.GetTopic(String.Format("{0}{1}", _configuration.TopicPrefix, TopicName));
            _producer =
                _session.CreateProducer(topic);
            _consumer = _session.CreateConsumer(topic);
            _consumer.Listener += OnMessageReceived;
            _connection.Start();
        }

        private void OnMessageReceived(IMessage message)
        {
            _bus.Publish(ActiveMqMessage.FromMessage(message));
        }
    }
}