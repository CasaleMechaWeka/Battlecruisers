using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Localisation;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class PrefabCacheTestGod : MonoBehaviour
    {
        async void Start()
        {
            Debug.Log("About to load the world :D");

            ILocTable commonLocTable = await LocTableFactory.Instance.LoadCommonTable();
            PrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory(commonLocTable);
            IPrefabCache cache = await prefabCacheFactory.CreatePrefabCacheAsync(new PrefabFetcher());

            Debug.Log("Finished loading the world :)");
        }
    }
}