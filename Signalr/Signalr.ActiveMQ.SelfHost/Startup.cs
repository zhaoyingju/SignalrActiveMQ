using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using SignalR.ActiveMQ;

[assembly: OwinStartupAttribute(typeof(SignalR.ActiveMQ.Sample1.Startup))]
namespace SignalR.ActiveMQ.Sample1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
