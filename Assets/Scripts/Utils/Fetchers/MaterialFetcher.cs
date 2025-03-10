using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    public class MaterialFetcher : IMaterialFetcher
    {
        public async Task<Material> GetMaterialAsync(string materialName)
        {
            AsyncOperationHandle<Material> handle = Addressables.LoadAssetAsync<Material>(materialName);
            await handle.Task;
            
            if (handle.Status != AsyncOperationStatus.Succeeded
                || handle.Result == null)
            {
                throw new ArgumentException("Failed to retrieve material: " + materialName);
            }

            return handle.Result;
        }
    }
}
