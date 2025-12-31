using System;
using BattleCruisers.Buildables.Buildings;
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
        private BuildingCategory _buildingCategory;

        public BuildingCategory BuildingCategory
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

        public PvPBuildingKey(BuildingCategory category, string prefabFileName)
            : base(prefabFileName, PvPBuildableType.Building)
        {
            BuildingCategory = category;
        }

        private string BuildingCategoryToFolderName(BuildingCategory buildingCategory)
        {
            switch (buildingCategory)
            {
                case BuildingCategory.Factory:
                    return PvPBuildingFolderNames.FACTORIES;
                case BuildingCategory.Tactical:
                    return PvPBuildingFolderNames.TACTICAL;
                case BuildingCategory.Defence:
                    return PvPBuildingFolderNames.DEFENCE;
                case BuildingCategory.Offence:
                    return PvPBuildingFolderNames.OFFENCE;
                case BuildingCategory.Ultra:
                    return PvPBuildingFolderNames.ULTRAS;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
