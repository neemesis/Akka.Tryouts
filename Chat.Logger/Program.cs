using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Chat.Common;

namespace Chat.Logger {
    class Program {
        static void Main(string[] args) {
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

            using (var system = ActorSystem.Create("MyLogger", config)) {
                var logger = system.ActorOf(Props.Create<LogActor>());

                Console.ReadLine();
            }

        }
    }

    class LogActor : ReceiveActor {
        private readonly ActorSelection _server = Context.ActorSelection("akka.tcp://MyServer@localhost:8081/user/ChatServer");

        public LogActor() {
            _server.Tell(new ConnectRequest { Username = "Logger" });
            Receive<object>(message =>
            {
                Console.WriteLine("==================");
                Console.WriteLine(Sender.Path);
                Console.WriteLine("Logger: {0}", Newtonsoft.Json.JsonConvert.SerializeObject(message));
                //Sender.Tell(message);
            });
        }
    }
}
