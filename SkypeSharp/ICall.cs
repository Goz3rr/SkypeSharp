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

    public interface ICall : ISkypeObject {
        void Finish();
    }
}
