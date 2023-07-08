using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    [Serializable]
    public class CaptainExoKey : PrefabKey
    {
        private const string prefabPathPreFix = "Prefabs/ScreensScene/ProfileScreen/Captains";

        protected override string PrefabPathPrefix
        {
            get
            {
                return prefabPathPreFix + PATH_SEPARATOR;
            }
        }

        public CaptainExoKey(string prefabName) : base(prefabName)
        {
        }
    }
}
