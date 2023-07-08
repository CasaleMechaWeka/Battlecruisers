using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public class PvPLevelInfo : IPvPLevelInfo
    {
        private readonly IGameModel _gameModel;
        private readonly IPvPPrefabFactory _prefabFactory;

        public IPvPCruiserController AICruiser { get; }
        public IPvPCruiserController PlayerCruiser { get; }

        public PvPLevelInfo(
            IPvPCruiserController aiCruiser,
            IPvPCruiserController playerCruiser,
            IGameModel gameModel,
            IPvPPrefabFactory prefabFactory)
        {
            PvPHelper.AssertIsNotNull(aiCruiser, playerCruiser, gameModel, prefabFactory);

            AICruiser = aiCruiser;
            PlayerCruiser = playerCruiser;
            _gameModel = gameModel;
            _prefabFactory = prefabFactory;
        }

        public bool CanConstructBuilding(PvPBuildingKey buildingKey)
        {
            IPvPBuilding building = _prefabFactory.GetBuildingWrapperPrefab(buildingKey).Buildable;

            return
                _gameModel.IsBuildingUnlocked(new BattleCruisers.Data.Models.PrefabKeys.BuildingKey(convertPvPBuildingCategory2PvEBuildingCategory(buildingKey.BuildingCategory), buildingKey.PrefabName))
                && building.NumOfDronesRequired <= AICruiser.DroneManager.NumOfDrones;
        }

        public IList<PvPBuildingKey> GetAvailableBuildings(PvPBuildingCategory category)
        {
            IList<BuildingKey> iKeys = _gameModel.GetUnlockedBuildings(convertPvPBuildingCategory2PvEBuildingCategory(category));
            return convertPvEBuildingKey2PvPBuildingKey(iKeys);
        }

        private IList<PvPBuildingKey> convertPvEBuildingKey2PvPBuildingKey(IList<BuildingKey> keys)
        {
            IList<PvPBuildingKey> iPvPKeys = new List<PvPBuildingKey>();
            foreach (BuildingKey key in keys)
            {
                iPvPKeys.Add(new PvPBuildingKey(convertPvEBuildingCategory2PvPBuildingCategory(key.BuildingCategory), "PvP" + key.PrefabName));
            }

            return iPvPKeys;
        }

        private PvPBuildingCategory convertPvEBuildingCategory2PvPBuildingCategory(BuildingCategory category)
        {
            switch (category)
            {
                case BuildingCategory.Ultra:
                    return PvPBuildingCategory.Ultra;
                case BuildingCategory.Tactical:
                    return PvPBuildingCategory.Tactical;
                case BuildingCategory.Factory:
                    return PvPBuildingCategory.Factory;
                case BuildingCategory.Offence:
                    return PvPBuildingCategory.Offence;
                case BuildingCategory.Defence:
                    return PvPBuildingCategory.Defence;
                default:
                    throw new System.Exception();
            }
        }
        private BuildingCategory convertPvPBuildingCategory2PvEBuildingCategory(PvPBuildingCategory category)
        {
            switch (category)
            {
                case PvPBuildingCategory.Ultra:
                    return BuildingCategory.Ultra;
                case PvPBuildingCategory.Tactical:
                    return BuildingCategory.Tactical;
                case PvPBuildingCategory.Factory:
                    return BuildingCategory.Factory;
                case PvPBuildingCategory.Offence:
                    return BuildingCategory.Offence;
                case PvPBuildingCategory.Defence:
                    return BuildingCategory.Defence;
                default:
                    throw new System.Exception();
            }
        }
    }
}
