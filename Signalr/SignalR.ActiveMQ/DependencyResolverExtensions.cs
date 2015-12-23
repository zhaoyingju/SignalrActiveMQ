using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.ActiveMQ
{
    public static class DependencyResolverExtensions
    {
        public static IDependencyResolver UseActiveMQ(this IDependencyResolver resolver, ActiveMqConnectionConfiguration configuration = null)
        {
            var config = configuration ?? new ActiveMqConnectionConfiguration();
            
            var bus = new Lazy<ActiveMqMessageBus>(() => new ActiveMqMessageBus(resolver, config));
            resolver.Register(typeof(IMessageBus), () => bus.Value);
            
            return resolver;
        }
    }
}
