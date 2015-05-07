using System;

namespace SkypeSharp {
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    public class MessageEventArgs : EventArgs {
        public readonly ChatMessage Message;

        public MessageEventArgs(ChatMessage message) {
            Message = message;
        }
    }
}