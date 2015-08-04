namespace SkypeSharp {
    public enum UserStatus {
        OnlineStatus,
        BuddyStatus,
        ReceivedAuthRequest
    }

    public interface IUser : ISkypeObject {
        string FullName { get; }
        string Language { get; }
        string Country { get; }
        string City { get; }

        void Authorize();
    }
}