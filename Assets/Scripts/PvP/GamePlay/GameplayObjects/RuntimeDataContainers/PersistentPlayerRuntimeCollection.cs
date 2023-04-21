using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Infrastructure;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Gameplay.GameplayObjects
{

    [CreateAssetMenu]
    public class PersistentPlayerRuntimeCollection : RuntimeCollection<PersistentPlayer>
    {
        public bool TryGetPlayer(ulong clientID, out PersistentPlayer persistentPlayer)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (clientID == Items[i].OwnerClientId)
                {
                    persistentPlayer = Items[i];
                    return true;
                }
            }

            persistentPlayer = null;
            return false;
        }
    }
}


