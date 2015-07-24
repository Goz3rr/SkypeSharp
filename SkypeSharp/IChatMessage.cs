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
        JoinedAsApplicant,
        Emoted
    }

    public interface IChatMessage : ISkypeObject {
        string Time { get; }
        string SenderHandle { get; }
        string SenderName { get; }
        string Body { get; }
        bool IsEditable { get; }

        IUser User { get; }
        string ChatName { get; }
        IChat Chat { get; }
        ChatMessageType Type { get; }

        void MarkAsSeen();
    }
}