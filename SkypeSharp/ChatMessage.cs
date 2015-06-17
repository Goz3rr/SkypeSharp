using System;

namespace SkypeSharp {
    public enum ChatMessageStatus {
        Unknown,
        Sending,
        Sent,
        Received,
        Read
    }

    public enum ChatMessageType {
        Unknown,
        SetTopic,
        Said,
        AddedMembers,
        SawMembers,
        CreatedChatWith,
        Left,
        PostedContacts,
        Gap_In_Chat,
        SetRole,
        Kicked,
        KickBanned,
        SetOptions,
        SetPicture,
        SetGuidelines,
        JoinedAsApplicant
    }

    /// <summary>
    ///     Class representing a Skype CHATMESSAGE object
    /// </summary>
    public class ChatMessage : SkypeObject {
        private string time;
        /// <summary>
        ///     Time of this message, UNIX timestamp
        /// </summary>
        public string Time {
            get { return time ?? (time = GetProperty("TIMESTAMP")); }
        }

        private string senderHandle;
        /// <summary>
        ///     Handle of the sender, same as <see cref="P:User.ID" />
        /// </summary>
        public string SenderHandle {
            get { return senderHandle ?? (senderHandle = GetProperty("FROM_HANDLE")); }
        }

        private string senderName;
        /// <summary>
        ///     Friendly name of the sender, same as <see cref="P:User.FullName" />
        /// </summary>
        public string SenderName {
            get { return senderName ?? (senderName = GetProperty("FROM_DISPNAME")); }
        }

        private string body;
        /// <summary>
        ///     Content of the message
        /// </summary>
        public string Body {
            get { return body ?? (body = GetProperty("BODY")); }
        }

        private User user;
        /// <summary>
        ///     User that sent the message
        /// </summary>
        public User User {
            get { return user ?? (user = new User(Skype, SenderHandle)); }
        }

        private string chatName;
        /// <summary>
        ///     Identifier of chat this message is from, same as <see cref="P:Chat.ID" />
        /// </summary>
        public string ChatName {
            get { return chatName ?? (chatName = GetProperty("CHATNAME")); }
        }

        private Chat chat;
        /// <summary>
        ///     Chat this message is from
        /// </summary>
        public Chat Chat {
            get { return chat ?? (chat = new Chat(Skype, ChatName)); }
        }

        private ChatMessageType? type;
        public ChatMessageType Type {
            get { return (ChatMessageType)(type ?? (type = (ChatMessageType)Enum.Parse(typeof(ChatMessageType), GetProperty("TYPE"), true))); }
        }

        public ChatMessage(Skype skype, string id) : base(skype, id, "CHATMESSAGE") {}

        /// <summary>
        ///     Mark message as seen
        /// </summary>
        public void MarkAsSeen() {
            SetProperty("SEEN");
        }
    }
}