namespace SkypeSharp {
    public interface IUser : ISkypeObject {
        string FullName { get; }
        string Language { get; }
        string Country { get; }
        string City { get; }
    }
}