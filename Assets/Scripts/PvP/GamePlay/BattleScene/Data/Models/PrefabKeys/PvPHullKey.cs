using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    [Serializable]
    public class PvPHullKey : PvPPrefabKey
    {
        private const string HULLS_FOLDER_NAME = "PvPHulls";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + HULLS_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public PvPHullKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
