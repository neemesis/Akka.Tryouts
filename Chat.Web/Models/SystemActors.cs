using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Akka.Actor;

namespace SignalRChat.Models {
    public static class SystemActors {
        public static IActorRef SignalRActor = ActorRefs.Nobody;

        public static IActorRef CommandProcessor = ActorRefs.Nobody;
    }

    public class ActorSystemRefs {
        public static ActorSystem ActorSystem;
    }
}