using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public class PvPPrefabContainer<TPrefab> : IPvPPrefabContainer<TPrefab> where TPrefab : class
    {
        public AsyncOperationHandle<GameObject> Handle { get; }
        public TPrefab Prefab { get; }

        public PvPPrefabContainer(AsyncOperationHandle<GameObject> handle, TPrefab prefab)
        {
            PvPHelper.AssertIsNotNull(handle, prefab);

            Handle = handle;
            Prefab = prefab;
        }
    }
}