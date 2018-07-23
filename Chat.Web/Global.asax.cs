using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Chat.Common;
using SignalRChat.Models;

namespace SignalRChat {
    public class MvcApplication : System.Web.HttpApplication {
        public static ActorSystem ActorSystem;
        //here you would store your toplevel actor-refs
        public static IActorRef MyActor;

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var config = ConfigurationFactory.ParseString(@"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        dot-netty.tcp {
		    port = 0
		    hostname = localhost
        }
    }
}
");

            ActorSystem = ActorSystem.Create("WebServer", config);
            MyActor = ActorSystem.ActorOf<BlaActor>("WebClient");
            MyActor.Tell(new ConnectRequest { Username = "TESTTT" });
        }
    }
}
