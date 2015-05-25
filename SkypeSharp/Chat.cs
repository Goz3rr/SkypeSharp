namespace SkypeSharp {
    public class Chat : SkypeObject {
        public Chat(Skype skype, string id) : base(skype, id, "CHAT") {}

        public void Send(string text) {
            Skype.Send("CHATMESSAGE " + ID + " " + text);
        }
    }
}