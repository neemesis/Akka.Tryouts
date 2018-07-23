using System;
using System.Web;
using Akka.Actor;
using Akka.Configuration;
using Chat.Client;
using Chat.Common;
using Microsoft.AspNet.SignalR;
namespace SignalRChat {
    public class ChatHub : Hub {
        public void Send(string name, string message) {
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

            //using (var system = ActorSystem.Create("WebServer", config)) {
            //    var client = system.ActorOf<BlaActor>("WebClient");

            //    client.Tell(new SayRequest { Username = name, Text = message });
            //}

            MvcApplication.MyActor.Tell(new SayRequest { Username = name, Text = message });

            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
    }

    class BlaActor : ReceiveActor {
        private readonly ActorSelection _server = Context.ActorSelection("akka.tcp://MyServer@localhost:8081/user/ChatServer");
        //private readonly ActorSelection _server2 = Context.ActorSelection("akka.tcp://MyServer@169.254.80.80:8081/user/ChatServer");

        public BlaActor() {
            //_server.Tell(new ConnectRequest { Username = "Logger" });
            Receive<SayRequest>(message =>
            {
                _server.Tell(message);
                //_server2.Tell(message);
            });

            Receive<ConnectRequest>(msg => _server.Tell(msg));

            Receive<SayResponse>(msg => {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                hubContext.Clients.All.addNewMessageToPage(msg.Username, msg.Text);
            });
        }
    }
}