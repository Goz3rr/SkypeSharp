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

    public interface IChatMember : ISkypeObject {
        string ChatName { get; }
        string Identity { get; }
        ChatRole Role { get; set; }
        bool Active { get; }
    }
}