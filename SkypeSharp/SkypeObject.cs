﻿using System;

namespace SkypeSharp {
    public class SkypeErrorException : Exception {
        public SkypeErrorException() {}
        public SkypeErrorException(string message) : base(message) {}
    }

    /// <summary>
    ///     Class for representing Skype API objects, provides Get and Set methods for properties
    /// </summary>
    public abstract class SkypeObject {
        /// <summary>
        ///     Skype internal ID
        /// </summary>
        public string ID { get; protected set; }

        /// <summary>
        ///     Name of Skype Object (USER, CHATMESSAGE, CALL, etc)
        /// </summary>
        protected readonly string Name;

        /// <summary>
        ///     Skype DBUS interface
        /// </summary>
        protected readonly Skype Skype;

        protected SkypeObject(Skype skype, string id, string name) {
            Skype = skype;
            ID = id;
            Name = name;
        }

        /// <summary>
        ///     Get a property from this object
        /// </summary>
        /// <param name="property">Arguments, joined with spaces</param>
        /// <returns>Skype response</returns>
        protected string GetProperty(params string[] property) {
            string args = Name + " " + ID + " " + String.Join(" ", property);
            string response = Skype.Send("GET " + args);
            return response.Substring(args.Length + 1);
        }

        /// <summary>
        ///     Set a property of this object
        /// </summary>
        /// <param name="property">Arguments, joined with spaces</param>
        protected string SetProperty(params string[] property) {
            string message = Name + " " + ID + " " + String.Join(" ", property);
            return Skype.Send(message);
        }

        /// <summary>
        ///     Alter a property of this object
        /// </summary>
        /// <param name="property">Arguments, joined with spaces</param>
        protected void Alter(params string[] property) {
            string message = "ALTER " + Name + " " + ID + " " + String.Join(" ", property);
            if(Skype.Send(message) != message) throw new SkypeErrorException(message);
        }
    }
}