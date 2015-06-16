using System;

namespace SkypeSharp {
    public enum ChatRole {
        Unknown,
        Creator,
        Master,
        Helper,
        User,
        Listener,
        Applicant
    }

    public class ChatMember : SkypeObject {
        private string chatName;
        /// <summary>
        ///     Handle? of the chat
        /// </summary>
        public string ChatName {
            get { return chatName ?? (chatName = GetProperty("CHATNAME")); }
        }

        private string identity;
        /// <summary>
        ///     Username? of the chat member
        /// </summary>
        public string Identity {
            get { return identity ?? (identity = GetProperty("IDENTITY")); }
        }

        private ChatRole? role;
        /// <summary>
        ///     Role of member in chat
        /// </summary>
        public ChatRole Role {
            get { return (ChatRole)(role ?? (role = (ChatRole)Enum.Parse(typeof(ChatRole), GetProperty("ROLE"), true))); }
            set {
                if(role == value || value == ChatRole.Unknown) return;
                Alter("SETROLETO", value.ToString().ToUpper());
            }
        }

        private bool? active;
        /// <summary>
        ///     Has member succesfully joined the chat
        /// </summary>
        public bool Active {
            get { return (bool)(active ?? (active = GetBoolProperty("IS_ACTIVE"))); }
        }

        public ChatMember(Skype skype, string id) : base(skype, id, "CHATMEMBER") {}
    }
}