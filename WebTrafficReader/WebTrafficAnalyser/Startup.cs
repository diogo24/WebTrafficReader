using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using TrafficReaderService;

[assembly: OwinStartup(typeof(WebTrafficAnalyser.Startup))]

namespace WebTrafficAnalyser
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //var trafficReaderService = new FiddlerCoreService();
            //trafficReaderService.Start();

            //var traffic2 = new FiddlerCoreServiceV2();
            //traffic2.Start();
        }
    }
}
