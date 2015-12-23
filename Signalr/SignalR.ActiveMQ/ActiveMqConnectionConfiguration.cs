using System;
using System.Configuration;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.ActiveMQ
{
    public class ActiveMqConnectionConfiguration : ScaleoutConfiguration
    {
        private const string ConfigPrefix = "signalr-activemq:";
        private const string BrokerUrlKey = "brokerUrl";
        private const string DefaultTopicPrefix = "SignalR.";

        public ActiveMqConnectionConfiguration(string connectionString = null, string topicPrefix = null,
            TimeSpan? timeToLive = null)
        {
            ConnectionString = string.IsNullOrEmpty(connectionString)
                ? ConfigurationManager.AppSettings[ConfigPrefix + BrokerUrlKey]
                : connectionString;

            TopicPrefix = string.IsNullOrEmpty(topicPrefix) ? DefaultTopicPrefix : topicPrefix;

            TimeToLive = timeToLive ?? TimeSpan.FromSeconds(30);
        }

        public string ConnectionString { get; set; }

        public TimeSpan TimeToLive { get; set; }

        public string TopicPrefix { get; set; }
    }
}