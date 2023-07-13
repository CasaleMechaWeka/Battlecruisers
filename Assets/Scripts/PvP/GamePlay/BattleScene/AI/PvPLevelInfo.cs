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
        private PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        private readonly IPvPPrefabFactory _prefabFactory;

        public PvPCruiser AICruiser { get; }
        public PvPCruiser PlayerCruiser { get; }

        public PvPLevelInfo(
            PvPCruiser aiCruiser,
            PvPCruiser playerCruiser,
            PvPBattleSceneGodTunnel battleSceneGodTunnel,
            IPvPPrefabFactory prefabFactory)
        {
            PvPHelper.AssertIsNotNull(aiCruiser, playerCruiser, battleSceneGodTunnel, prefabFactory);

            AICruiser = aiCruiser;
            PlayerCruiser = playerCruiser;
            _battleSceneGodTunnel = battleSceneGodTunnel;
            _prefabFactory = prefabFactory;           
        }

        public bool CanConstructBuilding(PvPBuildingKey buildingKey)
        {
            IPvPBuilding building = _prefabFactory.GetBuildingWrapperPrefab(buildingKey).Buildable;
            return
                AICruiser.Faction == Buildables.PvPFaction.Blues ?
                _battleSceneGodTunnel.IsBuildingUnlocked_LeftPlayer(buildingKey) : _battleSceneGodTunnel.IsBuildingUnlocked_RightPlayer(buildingKey)
                && building.NumOfDronesRequired <= AICruiser.DroneManager.NumOfDrones;
        }

        public IList<PvPBuildingKey> GetAvailableBuildings(PvPBuildingCategory category)
        {
            return AICruiser.Faction == Buildables.PvPFaction.Blues? _battleSceneGodTunnel.GetUnlockedBuildings_LeftPlayer(category) : _battleSceneGodTunnel.GetUnlockedBuildings_RightPlayer(category);             
        }
    }
}
