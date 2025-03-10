using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    public class PrefabContainer<TPrefab> : IPrefabContainer<TPrefab> where TPrefab : class
    {
        public AsyncOperationHandle<GameObject> Handle { get; }
        public TPrefab Prefab { get; }

        public PrefabContainer(AsyncOperationHandle<GameObject> handle, TPrefab prefab)
        {
            Helper.AssertIsNotNull(handle, prefab);

            Handle = handle;
            Prefab = prefab;
        }
    }
}