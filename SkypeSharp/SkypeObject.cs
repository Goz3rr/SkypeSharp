using System;

namespace SkypeSharp {
    public class SkypeErrorException : Exception {
        public SkypeErrorException() {}
        public SkypeErrorException(string message) : base(message) {}
    }

    public abstract class SkypeObject {
        public string ID { get; protected set; }

        protected readonly string Name;
        protected readonly Skype Skype;

        protected SkypeObject(Skype skype, string id, string name) {
            Skype = skype;
            ID = id;
            Name = name;
        }

        protected string GetProperty(params string[] property) {
            string args = Name + " " + ID + " " + String.Join(" ", property);
            string response = Skype.Send("GET " + args);
            return response.Substring(args.Length + 1);
        }

        protected void SetProperty(string property, string value) {
            string message = "SET " + Name + " " + ID + " " + property + " " + value;
            if(Skype.Send(message) != message) throw new SkypeErrorException(message);
        }

        protected void Alter(params string[] property) {
            string message = "ALTER " + Name + " " + ID + " " + String.Join(" ", property);
            if(Skype.Send(message) != message) throw new SkypeErrorException(message);
        }
    }
}