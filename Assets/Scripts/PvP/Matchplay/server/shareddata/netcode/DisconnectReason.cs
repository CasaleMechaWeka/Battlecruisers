using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BattleCruisers.Network.Multiplay.Matchplay.Networking
{
    public class DisconnectReason
    {
        public void SetDisconnectReason(ConnectStatus reason)
        {
            Debug.Assert(reason != ConnectStatus.Success);
            Reason = reason;
        }

        public ConnectStatus Reason { get; private set; } = ConnectStatus.Undefined;

        public void Clear()
        {
            Reason = ConnectStatus.Undefined;
        }

        public bool HasTransitionReason => Reason != ConnectStatus.Undefined;
    }
}

