using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Signalr.ActiveMQ;
using System.Threading.Tasks;

namespace SignalR.ActiveMQ.Sample.Signal.Class
{   
    public class chatHub : Hub
    {
        public void Send(string clientName, string message)
        {
            Procucer.GetInstance().Send(message);            
        }
        public override Task OnConnected()
        {
            Customer.Clients = this.Clients;
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Customer.Clients = this.Clients;
            return base.OnDisconnected(stopCalled);
        }
    }
}