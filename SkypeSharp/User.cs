﻿namespace SkypeSharp {
    /// <summary>
    ///     Class representing a Skype USER object
    /// </summary>
    public class User : SkypeObject {
        private string fullName;

        /// <summary>
        ///     Friendly name
        /// </summary>
        public string FullName {
            get { return fullName ?? (fullName = GetProperty("FULLNAME")); }
        }

        private string language;

        /// <summary>
        ///     Language as set by the user
        /// </summary>
        public string Language {
            get { return language ?? (language = GetProperty("LANGUAGE")); }
        }

        private string country;

        /// <summary>
        ///     Country as set by the user
        /// </summary>
        public string Country {
            get { return country ?? (country = GetProperty("COUNTRY")); }
        }

        private string city;

        /// <summary>
        ///     Country as set by the user
        /// </summary>
        public string City {
            get { return city ?? (city = GetProperty("CITY")); }
        }

        public User(Skype skype, string id) : base(skype, id, "USER") {}
    }
}