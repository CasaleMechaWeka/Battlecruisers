using BattleCruisers.Data.Models;
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
                _gameModel.IsBuildingUnlocked(buildingKey)
                && building.NumOfDronesRequired <= AICruiser.DroneManager.NumOfDrones;
        }

        public IList<PvPBuildingKey> GetAvailableBuildings(PvPBuildingCategory category)
        {
            return _gameModel.GetUnlockedBuildings(category);
        }
    }
}
