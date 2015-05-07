using System;

using SkypeSharp;

namespace SkypeTest {
    internal class Program {
        private static void Main(string[] args) {
            Skype skype = new Skype("skypetestthing");
            Console.WriteLine(skype.Attach());
            Console.WriteLine(skype.GetVersion());

            skype.OnMessageReceived += delegate(object sender, MessageEventArgs e) {
                if(e.Message.SenderHandle != "gozbot") {
                    //Console.WriteLine(e.Message.Body);
                    e.Message.Chat.Send("Got your message: " + e.Message.Body);
                }
            };

            skype.Listen();
        }
    }
}