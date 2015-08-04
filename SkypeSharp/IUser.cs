namespace SkypeSharp {
    public enum UserStatus {
        OnlineStatus,
        BuddyStatus,
        ReceivedAuthRequest,
        IsAuthorized,
        IsBlocked,
        Timezone,
        NROF_AUTHED_BUDDIES
    }

    public interface IUser : ISkypeObject {
        string FullName { get; }
        string Language { get; }
        string Country { get; }
        string City { get; }

        void Authorize();
    }
}