using System.Collections.Generic;
using System.Linq;

namespace SkypeSharp {
    public class Chat : SkypeObject {
        public IEnumerable<User> Users {
            get {
                string[] usernames = GetProperty("MEMBERS").Split(' ');
                return usernames.Select(u => new User(Skype, u));
            }
        }

        public Chat(Skype skype, string id) : base(skype, id, "CHAT") {}

        public void Send(string text) {
            Skype.Send("CHATMESSAGE " + ID + " " + text);
        }
    }
}