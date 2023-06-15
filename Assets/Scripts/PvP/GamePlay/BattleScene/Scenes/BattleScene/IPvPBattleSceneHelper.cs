using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data.Models;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public interface IPvPBattleSceneHelper
    {
        IPvPPrefabKey PlayerACruiser { get; }
        IPvPPrefabKey PlayerBCruiser { get; }
        IPvPBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        IPvPLevel GetPvPLevel();
        ILoadout GetPlayerLoadout();
        IPvPSlotFilter CreateHighlightableSlotFilter();
        IPvPBuildProgressCalculator CreatePlayerACruiserBuildProgressCalculator();
        IPvPBuildProgressCalculator CreatePlayerBCruiserBuildProgressCalculator();
        IPvPBuildProgressCalculator CreateAICruiserBuildProgressCalculator();
        IPvPUIManager CreateUIManager();
        void InitialiseUIManager(PvPManagerArgs args);
        IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(IPvPDroneManager droneManager);
        IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(PvPCruiser playerCruiser);
        IPvPPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(IPvPCruiser playerCruiser);
        IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(
                             IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager
                             // IPvPPrioritisedSoundPlayer soundPlayer,
                             //  IPvPTargetIndicator targetIndicator
                             );
        IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(
            IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager,
            IPvPPrioritisedSoundPlayer soundPlayer,
            IPvPTargetIndicator targetIndicator
   );
        Task<IPvPPrefabContainer<PvPBackgroundImageStats>> GetBackgroundStatsAsync(int levelNum);
    }
}

