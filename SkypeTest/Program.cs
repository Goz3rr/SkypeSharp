using System;

using SkypeSharp;

namespace SkypeTest {
    internal class Program {
        private static void Main(string[] args) {
            Skype skype = new Skype("skypetestthing");
            Console.WriteLine(skype.Attach());
            Console.WriteLine(skype.GetVersion());

            skype.OnMessageStatusChanged += delegate(object sender, ChatMessage message, ChatMessageStatus status) {
                if(message.SenderHandle != "gozbot") {
                    //Console.WriteLine(e.Message.Body);
                    message.Chat.Send("Got your message: " + message.Body);
                }
            };

            skype.Listen();
        }
    }
}