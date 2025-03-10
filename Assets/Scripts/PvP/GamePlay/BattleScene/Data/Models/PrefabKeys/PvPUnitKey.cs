using BattleCruisers.Buildables.Units;
using System;
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
        private UnitCategory _unitCategory;

        public UnitCategory UnitCategory
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

        public PvPUnitKey(UnitCategory category, string prefabFileName)
            : base(prefabFileName, PvPBuildableType.Unit)
        {
            UnitCategory = category;
        }

        private string UnitCategoryToFolderName(UnitCategory unitCategory)
        {
            switch (unitCategory)
            {
                case UnitCategory.Aircraft:
                    return PvPUnitFolderNames.AIRCRAFT;
                case UnitCategory.Naval:
                    return PvPUnitFolderNames.NAVAL;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
