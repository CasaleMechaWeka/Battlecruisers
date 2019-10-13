using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class PrefabCacheTestGod : MonoBehaviour
    {
        async void Start()
        {
            Debug.Log("About to load the world :D");

            PrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory();
            IPrefabCache cache = await prefabCacheFactory.CreatePrefabCacheAsync(new PrefabFetcher());

            Debug.Log("Finished loading the world :)");
        }
    }
}