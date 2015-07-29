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

    /// <summary>
    ///     Skype wrapper
    /// </summary>
    public class Skype : ISkype {
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

        /// <summary>
        ///     Friendly name to be used when connecting to skype
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Protocol version, Defaults to 5
        /// </summary>
        public int Protocol { get; private set; }

        /// <summary>
        ///     Is API attached to Skype
        /// </summary>
        public bool Attached { get; private set; }

        /// <summary>
        ///     Event invoked when CHATMESSAGE status changes (Sent, Received, Read)
        /// </summary>
        public event MessageEventHandler OnMessageStatusChanged;

        /// <summary>
        ///     Event invoked when CALL status changes
        /// </summary>
        public event CallEventHandler OnCallStatusChanged;

        public Skype(string name, int protocol = 5) {
            Name = name;
            Protocol = protocol;
        }

        /// <summary>
        ///     Send raw message over DBUS
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <returns>Response from Skype</returns>
        public string Send(string message) {
            return skypeSend.Invoke(message);
        }

        /// <summary>
        ///     Attach to Skype
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Start listening for DBUS messages, blocks thread until detached
        /// </summary>
        public void Listen() {
            while(Attached) {
                Bus.Session.Iterate();
                Thread.Sleep(1);
            }
        }

        /// <summary>
        ///     Get a Skype property
        /// </summary>
        /// <param name="property">Arguments, joined with spaces</param>
        /// <returns>Property value</returns>
        public string GetProperty(params string[] property) {
            string args = String.Join(" ", property);
            string response = Send("GET " + args);
            return response.Substring(args.Length + 1);
        }

        /// <summary>
        ///     Set a Skype property
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">New value</param>
        public void SetProperty(string name, string value) {
            Send(String.Format("SET {0} {1}", name, value));
        }

        /// <summary>
        ///     Get skype version
        /// </summary>
        /// <returns>Skype version</returns>
        public string GetVersion() {
            return GetProperty("SKYPEVERSION");
        }

        public IChat NewChat(string id) {
            return new Chat(this, id);
        }

        public IUser NewUser(string id) {
            return new User(this, id);
        }
    }
}