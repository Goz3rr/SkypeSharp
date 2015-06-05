namespace SkypeSharp {
    public class User : SkypeObject {
        private string fullName;
        public string FullName {
            get { return fullName ?? (fullName = GetProperty("FULLNAME")); }
        }

        private string language;
        public string Language {
            get { return language ?? (language = GetProperty("LANGUAGE")); }
        }

        private string country;
        public string Country {
            get { return country ?? (country = GetProperty("COUNTRY")); }
        }

        private string city;
        public string City {
            get { return city ?? (city = GetProperty("CITY")); }
        }

        public User(Skype skype, string id) : base(skype, id, "USER") {}
    }
}