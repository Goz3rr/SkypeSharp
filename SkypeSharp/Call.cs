namespace SkypeSharp {
    /// <summary>
    ///     Class representing a Skype CALL object
    /// </summary>
    public class Call : SkypeObject, ICall {
        public Call(ISkype skype, string id) : base(skype, id, "CALL") { }

        /// <summary>
        ///     Hang up call
        /// </summary>
        public void Finish() {
            Alter("END", "HANGUP");
        }
    }
}