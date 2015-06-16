using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        /// <summary>
        ///     List of chatmembers, useful for changing roles
        ///     Skype broke this so it probably doesn't work
        /// </summary>
        public IEnumerable<ChatMember> ChatMembers {
            get {
                string[] members = GetProperty("MEMBEROBJECTS").Split(' ');
                return members.Select(m => new ChatMember(Skype, m));
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

        /// <summary>
        ///     Uses xdotool to attempt to send skype a message
        /// </summary>
        /// <param name="text"></param>
        public void SendRaw(string text) {
            Skype.Send("OPEN CHAT " + ID);

            using(StreamWriter s = new StreamWriter("clipboard.tmp")) {
                s.Write(text);
            }

            Process xclip = new Process();
            xclip.StartInfo.FileName = "/usr/bin/xclip";
            xclip.StartInfo.Arguments = "-i clipboard.tmp -selection clipboard";
            xclip.Start();
            xclip.WaitForExit();
            
            Process xdo = new Process();
            xdo.StartInfo.FileName = "/usr/bin/xdotool";
            xdo.StartInfo.Arguments = "search --name skype key ctrl+v+ctrl+shift+Return";
            xdo.Start();
            xdo.WaitForExit();
        }
    }
}