namespace SkypeSharp {
    public enum CallStatus {
        Unknown,
        Not_Available,
        Unplaced,
        Routing,
        EarlyMedia,
        Failed,
        Ringing,
        InProgress,
        OnHold,
        Finished,
        Missed,
        Refused,
        Busy,
        Cancelled,
        Redial_Pending,
        RemoteHold,
        VM_BUFFERING_GREETING,
        VM_PLAYING_GREETING,
        VM_RECORDING,
        VM_UPLOADING,
        VM_SENT,
        VM_CANCELLED,
        VM_FAILED,
        Transferring,
        Transferred
    }

    /// <summary>
    ///     Class representing a Skype CALL object
    /// </summary>
    public class Call : SkypeObject {
        public Call(Skype skype, string id) : base(skype, id, "CALL") { }

        /// <summary>
        ///     Hang up call
        /// </summary>
        public void Finish() {
            Alter("END", "HANGUP");
        }
    }
}