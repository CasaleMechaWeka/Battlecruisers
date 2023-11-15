using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    [Serializable]
    public class PvPBuildingKey : PvPBuildableKey
    {
        private static class PvPBuildingFolderNames
        {
            public const string FACTORIES = "Factories";
            public const string TACTICAL = "Tactical";
            public const string DEFENCE = "Defence";
            public const string OFFENCE = "Offence";
            public const string ULTRAS = "Ultras";
        }

        [SerializeField]
        private PvPBuildingCategory _buildingCategory;

        public PvPBuildingCategory BuildingCategory
        {
            get { return _buildingCategory; }
            private set { _buildingCategory = value; }
        }

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + BuildingCategoryToFolderName(BuildingCategory) + PATH_SEPARATOR;
            }
        }

        public PvPBuildingKey(PvPBuildingCategory category, string prefabFileName)
            : base(prefabFileName, PvPBuildableType.Building)
        {
            BuildingCategory = category;
        }

        private string BuildingCategoryToFolderName(PvPBuildingCategory buildingCategory)
        {
            switch (buildingCategory)
            {
                case PvPBuildingCategory.Factory:
                    return PvPBuildingFolderNames.FACTORIES;
                case PvPBuildingCategory.Tactical:
                    return PvPBuildingFolderNames.TACTICAL;
                case PvPBuildingCategory.Defence:
                    return PvPBuildingFolderNames.DEFENCE;
                case PvPBuildingCategory.Offence:
                    return PvPBuildingFolderNames.OFFENCE;
                case PvPBuildingCategory.Ultra:
                    return PvPBuildingFolderNames.ULTRAS;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
