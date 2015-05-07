namespace SkypeSharp {
    public class ChatMessage : SkypeObject {
        public string Time { get; private set; }
        public string SenderHandle { get; private set; }
        public string SenderName { get; private set; }
        public string Body { get; private set; }
        public Chat Chat { get; private set; }

        public ChatMessage(Skype skype, string id) : base(skype, id, "CHATMESSAGE") {
            Time = GetProperty("TIMESTAMP");
            SenderHandle = GetProperty("FROM_HANDLE");
            SenderName = GetProperty("FROM_DISPNAME");
            Body = GetProperty("BODY");
            Chat = new Chat(skype, GetProperty("CHATNAME"));
        }
    }
}