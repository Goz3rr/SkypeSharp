using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SkypeSharp {
    /// <summary>
    ///     Class representing a Skype CHAT object
    /// </summary>
    public class Chat : SkypeObject, IChat {
        /// <summary>
        ///     List of users in this chat
        /// </summary>
        public IEnumerable<IUser> Users {
            get {
                string[] usernames = GetProperty("MEMBERS").Split(' ');
                return usernames.Select(u => new User(Skype, u));
            }
        }

        /// <summary>
        ///     List of chatmembers, useful for changing roles
        ///     Skype broke this so it probably doesn't work
        /// </summary>
        public IEnumerable<IChatMember> ChatMembers {
            get {
                string[] members = GetProperty("MEMBEROBJECTS").Split(' ');
                return members.Select(m => new ChatMember(Skype, m));
            }
        }

        public Chat(ISkype skype, string id) : base(skype, id, "CHAT") {}

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
        /// <param name="text">HTML formatted text to send</param>
        public void SendRaw(string text) {
            int count = 0;

            while(count < 2) {
                Skype.Send("OPEN CHAT " + ID);

                using(StreamWriter s = new StreamWriter("clipboard.tmp")) {
                    s.Write(text);
                }

                Process xclip = new Process {
                    StartInfo = {
                        FileName = "/usr/bin/xclip",
                        Arguments = "-i clipboard.tmp -selection clipboard"
                    }
                };
                xclip.Start();
                xclip.WaitForExit();

                Process xdo = new Process {
                    StartInfo = {
                        FileName = "/usr/bin/xdotool",
                        Arguments = "key ctrl+v+ctrl+shift+Return"
                    }
                };
                xdo.Start();
                xdo.WaitForExit();

                if(xdo.ExitCode != 0) {
                    Debug.WriteLine("xdotool returned " + xdo.ExitCode);
                    count++;
                } else break;
            }
        }
    }
}