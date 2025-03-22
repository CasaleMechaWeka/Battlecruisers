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

            ILocTable commonLocTable = await LocTableFactory.Instance.LoadCommonTableAsync();
            PrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory(commonLocTable);
            PrefabCache cache = await prefabCacheFactory.CreatePrefabCacheAsync();

            Debug.Log("Finished loading the world :)");
        }
    }
}