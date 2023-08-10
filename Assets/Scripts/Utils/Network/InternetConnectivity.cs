using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils.Network
{
    public class InternetConnectivity : INetworkState
    {
        public bool IsConnected { get; set; }     
        public InternetConnectivity(bool isConnected) { IsConnected = isConnected; }
    }
}

