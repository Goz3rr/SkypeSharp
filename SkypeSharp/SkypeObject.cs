using System;

namespace SkypeSharp {
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
            string args = Name + " " + ID + " " + property + " " + value;
            Skype.Send("SET " + args);
        }
    }
}