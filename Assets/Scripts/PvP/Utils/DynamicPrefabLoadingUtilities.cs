using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Utils
{
    /// <summary>
    /// A utilities class to handle the loading, tracking, and disposing of loaded network prefabs. Connection and
    /// disconnection payloads can be easily accessed from this class as well.
    /// </summary>
    public static class DynamicPrefabLoadingUtilities
    {
        const int k_EmptyDynamicPrefabHash = -1;

        public static int HashOfDynamicPrefabGUIDs { get; private set; } = k_EmptyDynamicPrefabHash;

        static Dictionary<AddressableGUID, AsyncOperationHandle<GameObject>> s_LoadedDynamicPrefabResourceHandles = new Dictionary<AddressableGUID, AsyncOperationHandle<GameObject>>(new AddressableGUIDEqualityComparer());

        static List<AddressableGUID> s_DynamicPrefabGUIDs = new List<AddressableGUID>();


        static Dictionary<int, HashSet<ulong>> s_PrefabHashToClientIds = new Dictionary<int, HashSet<ulong>>();

        public static bool HasClientLoadedPrefab(ulong clientId, int prefabHash) =>
            s_PrefabHashToClientIds.TryGetValue(prefabHash, out var clientIds) && clientIds.Contains(clientId);

        public static bool IsPrefabLoadedOnAllClients(AddressableGUID assetGuid) =>
            s_LoadedDynamicPrefabResourceHandles.ContainsKey(assetGuid);

        public static bool TryGetLoadedGameObjectFromGuid(AddressableGUID assetGuid, out AsyncOperationHandle<GameObject> loadedGameObject)
        {
            return s_LoadedDynamicPrefabResourceHandles.TryGetValue(assetGuid, out loadedGameObject);
        }

        public static Dictionary<AddressableGUID, AsyncOperationHandle<GameObject>> LoadedDynamicPrefabResourceHandles => s_LoadedDynamicPrefabResourceHandles;

        public static int LoadedPrefabCount => s_LoadedDynamicPrefabResourceHandles.Count;

        static NetworkManager s_NetworkManager;

        static DynamicPrefabLoadingUtilities() { }

        public static void Init(NetworkManager networkManager)
        {
            s_NetworkManager = networkManager;
        }


        public static void RecordThatClientHasLoadedAllPrefabs(ulong clientId)
        {
            foreach (var dynamicPrefabGUID in s_DynamicPrefabGUIDs)
            {
                RecordThatClientHasLoadedAPrefab(dynamicPrefabGUID.GetHashCode(), clientId);
            }
        }

        public static void RecordThatClientHasLoadedAPrefab(int assetGuidHash, ulong clientId)
        {
            if (s_PrefabHashToClientIds.TryGetValue(assetGuidHash, out var clientIds))
            {
                clientIds.Add(clientId);
            }
            else
            {
                s_PrefabHashToClientIds.Add(assetGuidHash, new HashSet<ulong>() { clientId });
            }
        }

        public static byte[] GenerateRequestPayload()
        {
            var payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                hashOfDynamicPrefabGUIDs = HashOfDynamicPrefabGUIDs
            });

            return System.Text.Encoding.UTF8.GetBytes(payload);
        }

        public static string GenerateDisconnectionPayload()
        {
            var rejectionPayload = new DisconnectionPayload()
            {
                reason = DisconnectReason.ClientNeedsToPreload,
                guids = s_DynamicPrefabGUIDs.Select(item => item.ToString()).ToList()
            };

            return JsonUtility.ToJson(rejectionPayload);
        }

        public static async Task<GameObject> LoadDynamicPrefab(AddressableGUID guid, int artificialDelayMilliseconds,
            bool recomputeHash = true)
        {
            if (s_LoadedDynamicPrefabResourceHandles.ContainsKey(guid))
            {
                Debug.Log($"Prefab has already been loaded, skipping loading this time | {guid}");
                return s_LoadedDynamicPrefabResourceHandles[guid].Result;
            }

            Debug.Log($"Loading dynamic prefab {guid.Value}");
            var op = Addressables.LoadAssetAsync<GameObject>(guid.ToString());
            var prefab = await op.Task;

#if ENABLE_ARTIFICIAL_DELAY
            // THIS IS FOR EDUCATIONAL PURPOSES AND SHOULDN'T BE INCLUDED IN YOUR PROJECT
            await Task.Delay(artificialDelayMilliseconds);
#endif

            s_NetworkManager.AddNetworkPrefab(prefab);
            s_LoadedDynamicPrefabResourceHandles.Add(guid, op);

            if (recomputeHash)
            {
                CalculateDynamicPrefabArrayHash();
            }

            return prefab;
        }

        public static async Task<IList<GameObject>> LoadDynamicPrefabs(AddressableGUID[] guids,
            int artificialDelaySeconds = 0)
        {
            var tasks = new List<Task<GameObject>>();

            foreach (var guid in guids)
            {
                tasks.Add(LoadDynamicPrefab(guid, artificialDelaySeconds, recomputeHash: false));
            }

            var prefabs = await Task.WhenAll(tasks);
            CalculateDynamicPrefabArrayHash();

            return prefabs;
        }

        public static void RefreshLoadedPrefabGuids()
        {
            s_DynamicPrefabGUIDs.Clear();
            s_DynamicPrefabGUIDs.AddRange(s_LoadedDynamicPrefabResourceHandles.Keys);
        }

        static void CalculateDynamicPrefabArrayHash()
        {

            RefreshLoadedPrefabGuids();
            s_DynamicPrefabGUIDs.Sort((a, b) => a.Value.CompareTo(b.Value));
            HashOfDynamicPrefabGUIDs = k_EmptyDynamicPrefabHash;


            unchecked
            {
                int hash = 17;
                for (var i = 0; i < s_DynamicPrefabGUIDs.Count; ++i)
                {
                    hash = hash * 31 + s_DynamicPrefabGUIDs[i].GetHashCode();
                }

                HashOfDynamicPrefabGUIDs = hash;
            }

            Debug.Log($"Calculated hash of dynamic prefabs: {HashOfDynamicPrefabGUIDs}");
        }

        public static void UnloadAndReleaseAllDynamicPrefabs()
        {
            HashOfDynamicPrefabGUIDs = k_EmptyDynamicPrefabHash;

            foreach (var handle in s_LoadedDynamicPrefabResourceHandles.Values)
            {
                s_NetworkManager.RemoveNetworkPrefab(handle.Result);
                Addressables.Release(handle);
            }

            s_LoadedDynamicPrefabResourceHandles.Clear();
        }
    }
}

