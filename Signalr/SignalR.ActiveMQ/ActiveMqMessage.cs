using System;
using System.IO;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using Message = Microsoft.AspNet.SignalR.Messaging.Message;

namespace SignalR.ActiveMQ
{
    internal static class ActiveMqMessage
    {
        public static IMessage ToMessage(Message message, ActiveMqConnectionConfiguration configuration)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            using (var stream = new MemoryStream())
            {
                message.WriteTo(stream);
                return new ActiveMQBytesMessage
                {
                    Content = stream.ToArray(),
                    NMSTimeToLive = configuration.TimeToLive
                };
            }
        }

        public static Message FromMessage(IMessage message)
        {
            var bytesMessage = message as IBytesMessage;
            if (bytesMessage == null)
            {
                throw new ArgumentNullException("message");
            }

            using (var stream = new MemoryStream(bytesMessage.Content))
            {
                return Message.ReadFrom(stream);
            }
        }
    }
}