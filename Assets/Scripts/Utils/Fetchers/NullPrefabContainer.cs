using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    /// <summary>
    /// For test scenes.  Allows Handle to be null.
    /// </summary>
    public class NullPrefabContainer<TPrefab> : IPrefabContainer<TPrefab> where TPrefab : class
    {
        public AsyncOperationHandle<GameObject> Handle { get; set; }
        public TPrefab Prefab { get; set; }
    }
}