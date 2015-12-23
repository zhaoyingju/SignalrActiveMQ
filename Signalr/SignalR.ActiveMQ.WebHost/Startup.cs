using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using SignalR.ActiveMQ;

[assembly: OwinStartupAttribute(typeof(SignalR.ActiveMQ.Sample.Startup))]
namespace SignalR.ActiveMQ.Sample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.MapSignalR();
            // GlobalHost.DependencyResolver.UseActiveMQ();
            Customer.Run();
        }
    }
}
