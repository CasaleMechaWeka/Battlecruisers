using BattleCruisers.Data.Models.PrefabKeys;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    public class PrefabFetcher : IPrefabFetcher
    {
        private const string PREFAB_ROOT_DIR = "Assets/Resources_moved/";
        private const string PREFAB_FILE_EXTENSION = ".prefab";

        public async Task<TPrefab> GetPrefabAsync<TPrefab>(IPrefabKey prefabKey) where TPrefab : class
        {
            string addressableKey = PREFAB_ROOT_DIR + prefabKey.PrefabPath + PREFAB_FILE_EXTENSION;

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);

            Logging.Log(Tags.PREFAB_CACHE_FACTORY, $"Pre await Addressables.LoadAssetAsync: {prefabKey.PrefabPath}");
            await handle.Task;
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, $"After await Addressables.LoadAssetAsync: {prefabKey.PrefabPath}");

            if (handle.Status != AsyncOperationStatus.Succeeded
                || handle.Result == null)
            {
                throw new ArgumentException("Failed to retrieve prefab: " + addressableKey);
            }

            TPrefab prefabObject = handle.Result.GetComponent<TPrefab>();
            if (prefabObject == null)
            {
                throw new ArgumentException($"Prefab does not contain a component of type: {typeof(TPrefab)}.  Addressable key: {addressableKey}");
            }
            return prefabObject;
        }
    }
}
