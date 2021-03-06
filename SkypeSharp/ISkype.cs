﻿namespace SkypeSharp {
    public delegate void MessageEventHandler(object sender, IChatMessage message, ChatMessageStatus status);
    public delegate void CallEventHandler(object sender, ICall call, CallStatus status);
    public delegate void UserEventHandler(object sender, IUser user, UserStatus status, string arg);

    public interface ISkype {
        string Name { get; }
        int Protocol { get; }
        bool Attached { get; }

        event MessageEventHandler OnMessageStatusChanged;
        event CallEventHandler OnCallStatusChanged;
        event UserEventHandler OnUserStatusChanged;

        string Send(string message);
        bool Attach();
        void Listen();

        string GetProperty(params string[] property);
        void SetProperty(string name, string value);
        string GetVersion();

        IChat NewChat(string id);
        IUser NewUser(string id);
    }
}
