using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    public class PrefabContainer<TPrefab> : IPrefabContainer<TPrefab> where TPrefab : class
    {
        public AsyncOperationHandle<TPrefab> Handle { get; }
        public TPrefab Prefab { get; }

        public PrefabContainer(AsyncOperationHandle<TPrefab> handle, TPrefab prefab)
        {
            Helper.AssertIsNotNull(handle, prefab);

            Handle = handle;
            Prefab = prefab;
        }
    }
}