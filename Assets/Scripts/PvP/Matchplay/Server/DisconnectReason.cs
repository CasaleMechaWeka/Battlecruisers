using UnityEngine;



namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public class DisconnectReason
    {
        public void SetDisconnectReason(MatchplayConnectStatus reason)
        {
            Debug.Assert(reason != MatchplayConnectStatus.Success);
            Reason = reason;
        }

        public MatchplayConnectStatus Reason { get; private set; } = MatchplayConnectStatus.Undefined;

        public void Clear()
        {
            Reason = MatchplayConnectStatus.Undefined;
        }

        public bool HasTransitionReason => Reason != MatchplayConnectStatus.Undefined;
    }
}

