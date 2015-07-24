using System.Collections.Generic;

namespace SkypeSharp {
    public interface IChat : ISkypeObject {
        IEnumerable<IUser> Users { get; }
        IEnumerable<IChatMember> ChatMembers { get; }

        void Send(string text);
        void SendRaw(string text);
    }
}