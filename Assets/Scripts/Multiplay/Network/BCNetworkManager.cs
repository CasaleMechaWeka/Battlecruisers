using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Infrastructure;

public class BCNetworkManager : NetworkManager, INetworkObject
{
    public void DestroyNetworkObject()
        {
            Destroy(gameObject);
        }
}
