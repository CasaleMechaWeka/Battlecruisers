using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    [Serializable]
    public class PvPUnitKey : PvPBuildableKey
    {
        private static class PvPUnitFolderNames
        {
            public const string NAVAL = "Naval";
            public const string AIRCRAFT = "Aircraft";
            public const string ULTRA = "Ultras";
        }

        [SerializeField]
        private PvPUnitCategory _unitCategory;

        public PvPUnitCategory UnitCategory
        {
            get { return _unitCategory; }
            private set { _unitCategory = value; }
        }

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + UnitCategoryToFolderName(UnitCategory) + PATH_SEPARATOR;
            }
        }

        public PvPUnitKey(PvPUnitCategory category, string prefabFileName)
            : base(prefabFileName, PvPBuildableType.Unit)
        {
            UnitCategory = category;
        }

        private string UnitCategoryToFolderName(PvPUnitCategory unitCategory)
        {
            switch (unitCategory)
            {
                case PvPUnitCategory.Aircraft:
                    return PvPUnitFolderNames.AIRCRAFT;
                case PvPUnitCategory.Naval:
                    return PvPUnitFolderNames.NAVAL;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
