using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    [Serializable]
    public abstract class PvPPrefabKey : IPvPPrefabKey
    {
        [SerializeField]
        private string _prefabName;


        private const string PREFABS_BASE_PATH = "Prefabs/PvP/BattleScene/";
        protected const char PATH_SEPARATOR = '/';

        protected virtual string PrefabPathPrefix
        {
            get
            {
                return PREFABS_BASE_PATH;
            }
        }

        public string PrefabPath
        {
            get
            {
                return PrefabPathPrefix + _prefabName;
            }
        }

        public string PrefabName
        {
            get
            {
                return _prefabName;
            }
        }

        protected PvPPrefabKey(string prefabName)
        {
            _prefabName = prefabName;
        }

        public override bool Equals(object obj)
        {
            PvPPrefabKey other = obj as PvPPrefabKey;
            return other != null
                && other.PrefabPath == PrefabPath;
        }

        public override int GetHashCode()
        {
            return PrefabPath.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() + " " + _prefabName;
        }
    }
}
