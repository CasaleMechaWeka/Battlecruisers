using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils.Network
{
    public interface INetworkState
    {
        bool IsConnected { get; set; }
    }
}

