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

    public delegate void MessageEventHandler(object sender, ChatMessage message, ChatMessageStatus status);
    public delegate void CallEventHandler(object sender, Call call, CallStatus status);

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
                if(OnNotified != null) OnNotified.Invoke(this, new NotifiedEventArgs(message));
            }
        }

        private readonly Connection dbus = Bus.Session;
        private ISkypeSend skypeSend;
        private SkypeResponseHandler skypeResponse;

        public string Name { get; private set; }
        public int Protocol { get; private set; }
        public bool Attached { get; private set; }

        public event MessageEventHandler OnMessageStatusChanged;
        public event CallEventHandler OnCallStatusChanged;

        public Skype(string name, int protocol = 5) {
            Name = name;
            Protocol = protocol;
        }

        public string Send(string message) {
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
                skypeResponse.OnNotified += OnNotified;
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

        private void OnNotified(object sender, NotifiedEventArgs e) {
            //string[] parts = e.Message.Split(' ');
            //Console.WriteLine(">> Got notified with command " + parts[0]);

            if (e.Message.StartsWith("CHATMESSAGE ")) {
                string data = e.Message.Substring("CHATMESSAGE ".Length);
                string[] split = data.Split(' ');

                if (split[1] == "STATUS") {
                    ChatMessage msg = new ChatMessage(this, split[0]);

                    ChatMessageStatus status;
                    Enum.TryParse(split[2], true, out status);

                    if(OnMessageStatusChanged != null) OnMessageStatusChanged.Invoke(this, msg, status);
                }
            } else if (e.Message.StartsWith("CALL ")) {
                string data = e.Message.Substring("CALL ".Length);
                string[] split = data.Split(' ');

                if (split[1] == "STATUS") {
                    Call call = new Call(this, split[0]);

                    CallStatus status;
                    Enum.TryParse(split[2], true, out status);

                    if(OnCallStatusChanged != null) OnCallStatusChanged.Invoke(this, call, status);
                }
            }
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

        public void SetProperty(string name, string value) {
            Send(String.Format("SET {0} {1}", name, value));
        }

        public string GetVersion() {
            return GetProperty("SKYPEVERSION");
        }
    }
}