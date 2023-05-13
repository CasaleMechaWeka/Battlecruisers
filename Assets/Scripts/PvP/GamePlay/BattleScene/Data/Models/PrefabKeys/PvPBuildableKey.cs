using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public enum PvPBuildableType
    {
        Building, Unit
    }

    [Serializable]
    public abstract class PvPBuildableKey : PvPPrefabKey
    {
        private static class PvPBuildableTypeFolderNames
        {
            public const string UNITS = "Units";
            public const string BUILDINGS = "Buildings";
        }

        [SerializeField]
        private PvPBuildableType _buildableType;

        private const string BUILDABLES_FOLDER_NAME = "Buildables";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + BUILDABLES_FOLDER_NAME + PATH_SEPARATOR + BuildableTypeToFolderName(_buildableType) + PATH_SEPARATOR;
            }
        }

        protected PvPBuildableKey(string prefabFileName, PvPBuildableType buildableType)
            : base(prefabFileName)
        {
            _buildableType = buildableType;
        }

        private string BuildableTypeToFolderName(PvPBuildableType buildableType)
        {
            switch (buildableType)
            {
                case PvPBuildableType.Building:
                    return PvPBuildableTypeFolderNames.BUILDINGS;
                case PvPBuildableType.Unit:
                    return PvPBuildableTypeFolderNames.UNITS;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
