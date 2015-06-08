using System.Collections.Generic;
using System.Linq;

namespace SkypeSharp {
    /// <summary>
    ///     Class representing a Skype CHAT object
    /// </summary>
    public class Chat : SkypeObject {
        /// <summary>
        ///     List of users in this chat
        /// </summary>
        public IEnumerable<User> Users {
            get {
                string[] usernames = GetProperty("MEMBERS").Split(' ');
                return usernames.Select(u => new User(Skype, u));
            }
        }

        public Chat(Skype skype, string id) : base(skype, id, "CHAT") {}

        /// <summary>
        ///     Send a message to this chat
        /// </summary>
        /// <param name="text">Text to send</param>
        public void Send(string text) {
            Skype.Send("CHATMESSAGE " + ID + " " + text);
        }
    }
}