using BattleCruisers.Data.Models.PrefabKeys;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    public static class PrefabFetcher
    {
        private const string PREFAB_ROOT_DIR = "Assets/Resources_moved/";
        private const string PREFAB_FILE_EXTENSION = ".prefab";

        //this blocks the main thread for every call; should therefore only be a fallback
        public static TPrefab GetPrefabSync<TPrefab>(IPrefabKey prefabKey) where TPrefab : class, IPrefab
        {
            // 1) Start the async load
            string addressableKey = PREFAB_ROOT_DIR + prefabKey.PrefabPath + PREFAB_FILE_EXTENSION;
            var handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);

            // 2) Safely block the main thread while letting Unity pump the async calls
            handle.WaitForCompletion();

            if (handle.Status != AsyncOperationStatus.Succeeded
                || handle.Result == null)
            {
                throw new Exception($"Failed to load prefab synchronously for key: {prefabKey}");
            }

            // 3) Perform any static initialisation
            TPrefab prefabObject = handle.Result.GetComponent<TPrefab>();
            //handle.Result.StaticInitialise();

            // 4) Return the loaded prefab
            return prefabObject;
        }

        public static async Task<PrefabContainer<TPrefab>> GetPrefabAsync<TPrefab>(IPrefabKey prefabKey) where TPrefab : class
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
            return new PrefabContainer<TPrefab>(handle, prefabObject);
        }
    }
}
