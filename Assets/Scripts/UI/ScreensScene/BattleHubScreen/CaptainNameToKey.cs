using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine.Assertions;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class CaptainNameToKey : ICaptainNameToKey
    {
        private readonly IDictionary<string, CaptainExoKey> _captainKey;

        public CaptainNameToKey(IList<IPrefabKey> keys, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(keys, prefabFactory);

            _captainKey = new Dictionary<string, CaptainExoKey>();

            foreach (CaptainExoKey key in keys)
            {
                CaptainExo captainPrefab = prefabFactory.GetCaptainExo(key);
                _captainKey.Add(captainPrefab.captainName, key);
            }
        }

        public IPrefabKey GetKey(string CaptainName)
        {
            Assert.IsTrue(_captainKey.ContainsKey(CaptainName));
            return _captainKey[CaptainName];
        }
    }
}