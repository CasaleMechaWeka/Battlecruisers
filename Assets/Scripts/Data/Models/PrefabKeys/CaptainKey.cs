using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class CaptainKey : PrefabKey
    {
        private const string prefabPathPreFix = "Prefabs/ScreensScene/ProfileScreen/Captains";

        protected override string PrefabPathPrefix
        {
            get
            {
                return prefabPathPreFix + PATH_SEPARATOR;
            }
        }

        public CaptainKey(string prefabName) : base(prefabName)
        {
        }
    }
}