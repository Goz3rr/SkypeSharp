namespace SkypeSharp {
    public enum ChatMessageStatus {
        Unknown,
        Sending,
        Sent,
        Received,
        Read
    }

    public class ChatMessage : SkypeObject {
        private string time;
        public string Time {
            get { return time ?? (time = GetProperty("TIMESTAMP")); }
        }

        private string senderHandle;
        public string SenderHandle {
            get { return senderHandle ?? (senderHandle = GetProperty("FROM_HANDLE")); }
        }

        private string senderName;
        public string SenderName {
            get { return senderName ?? (senderName = GetProperty("FROM_DISPNAME")); }
        }

        public string Body {
            get { return GetProperty("BODY"); }
        }

        private User user;
        public User User {
            get { return user ?? (user = new User(Skype, SenderHandle)); }
        }

        private string chatName;
        public string ChatName {
            get { return chatName ?? (chatName = GetProperty("CHATNAME")); }
        }

        private Chat chat;
        public Chat Chat {
            get { return chat ?? (chat = new Chat(Skype, ChatName)); }
        }

        public ChatMessage(Skype skype, string id) : base(skype, id, "CHATMESSAGE") {}

        public void MarkAsSeen() {
            SetProperty("SEEN");
        }
    }
}