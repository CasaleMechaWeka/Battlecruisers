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

        public CaptainNameToKey(IList<IPrefabKey> keys)
        {
            Helper.AssertIsNotNull(keys);
            _captainKey = new Dictionary<string, CaptainExoKey>();
            GenerateNameKey(keys);
        }

        private void GenerateNameKey(IList<IPrefabKey> keys)
        {
            foreach (CaptainExoKey key in keys)
            {
                CaptainExo captainPrefab = PrefabFactory.GetCaptainExo(key);
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