using System;

namespace SkypeSharp {
    public delegate void NotifiedEventHandler(object sender, NotifiedEventArgs e);

    public class NotifiedEventArgs : EventArgs {
        public readonly string Message;

        public NotifiedEventArgs(string message) {
            Message = message;
        }
    }
}