using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Configuration;
using System.Web.Http.Routing;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Cors;
using Microsoft.Win32;

namespace ArquivoBalancaAPI
{
    public partial class Service1 : ServiceBase
    {
        HttpSelfHostServer server;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {           
            var config = new HttpSelfHostConfiguration("http://localhost:1000");

            config.Routes.MapHttpRoute("Default", "{controller}/{caminho}", new { caminho = RouteParameter.Optional });

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.EnableCors(new EnableCorsAttribute("*", headers: "*", methods: "*"));

            server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
        }

        protected override void OnStop()
        {
        }

    }
}
