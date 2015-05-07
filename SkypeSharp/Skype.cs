using System;
using System.Threading;

using DBus;

using org.freedesktop.DBus;

namespace SkypeSharp {
    [Interface("com.Skype.API")]
    public interface ISkypeSend : Introspectable {
        string Invoke(string message);
    }

    [Interface("com.Skype.API.Client")]
    public interface ISkypeResponse : Introspectable {
        void Notify(string message);
    }

    public class Skype {
        private class SkypeResponseHandler : ISkypeResponse {
            public event NotifiedEventHandler OnNotified;

            public SkypeResponseHandler(Connection dbus) {
                dbus.Register(new ObjectPath("/com/Skype/Client"), this);
            }

            public string Introspect() {
                return "Introspect shit";
            }

            public void Notify(string message) {
                Console.WriteLine("<< " + message);

                if(OnNotified != null) OnNotified.Invoke(this, new NotifiedEventArgs(message));
            }
        }

        private readonly Connection dbus = Bus.Session;
        private ISkypeSend skypeSend;
        private SkypeResponseHandler skypeResponse;

        public string Name { get; private set; }
        public int Protocol { get; private set; }
        public bool Attached { get; private set; }

        public event MessageEventHandler OnMessageSent;
        public event MessageEventHandler OnMessageReceived;
        public event MessageEventHandler OnMessageRead;

        public Skype(string name, int protocol = 5) {
            Name = name;
            Protocol = protocol;
        }

        public string Send(string message) {
            Console.WriteLine(">> " + message);
            return skypeSend.Invoke(message);
        }

        public bool Attach() {
            if(dbus == null) {
                Console.WriteLine("Environment variable DBUS_SESSION_BUS_ADDRESS not set!");
                return false;
            }

            try {
                skypeSend = dbus.GetObject<ISkypeSend>("com.Skype.API", new ObjectPath("/com/Skype"));
                skypeResponse = new SkypeResponseHandler(dbus);
                skypeResponse.OnNotified += delegate(object sender, NotifiedEventArgs e) {
                    if(e.Message.StartsWith("CHATMESSAGE ")) {
                        string data = e.Message.Substring("CHATMESSAGE ".Length);
                        string[] split = data.Split(' ');

                        if(split[1] == "STATUS") {
                            ChatMessage msg = new ChatMessage(this, split[0]);

                            if(split[2] == "SENT") {
                                if(OnMessageSent != null) OnMessageSent.Invoke(this, new MessageEventArgs(msg));
                            } else if(split[2] == "RECEIVED") {
                                if(OnMessageReceived != null) OnMessageReceived.Invoke(this, new MessageEventArgs(msg));
                            } else if(split[2] == "READ") {
                                if(OnMessageRead != null) OnMessageRead.Invoke(this, new MessageEventArgs(msg));
                            }
                        }
                    }
                };
            } catch(Exception e) {
                Console.WriteLine("Skype DBUS initialization error: {0}", e);
                return false;
            }

            if(skypeSend == null) {
                Console.WriteLine("Failed to get skype dbus object");
                return false;
            }

            if(Send("NAME " + Name) != "OK") {
                Console.WriteLine("Failed to set NAME");
                return false;
            }

            if(Send("PROTOCOL " + Protocol) != "PROTOCOL " + Protocol) {
                Console.WriteLine("Skype didn't accept PROTOCOL " + Protocol);
                return false;
            }

            Attached = true;
            return true;
        }

        public void Listen() {
            while(Attached) {
                Bus.Session.Iterate();
                Thread.Sleep(1);
            }
        }

        public string GetProperty(params string[] property) {
            string args = String.Join(" ", property);
            string response = Send("GET " + args);
            return response.Substring(args.Length + 1);
        }

        public string GetVersion() {
            return GetProperty("SKYPEVERSION");
        }
    }
}