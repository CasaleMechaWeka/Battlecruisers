using BattleCruisers.AI;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Threading;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public interface IPvPBattleSceneHelper
    {
        IPrefabKey PlayerACruiser { get; }
        IPrefabKey PlayerBCruiser { get; }
        IPrefabKey AIBotCruiser { get; }
        string[] PvPHullNames { get; }
        IBuildingCategoryPermitter BuildingCategoryPermitter { get; }
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
        IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(IPvPCruiser playerCruiser);
        IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(
                             IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager
                             // IPrioritisedSoundPlayer soundPlayer,
                             //  IPvPTargetIndicator targetIndicator
                             );
        IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(
            IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager,
            IPrioritisedSoundPlayer soundPlayer,
            IPvPTargetIndicator targetIndicator
                                );
        IArtificialIntelligence CreateAI(PvPCruiser aiCruiser, PvPCruiser playerCruiser, int currentLevelNum);
        IManagedDisposable CreateDroneEventSoundPlayer(IPvPCruiser playerCruiser, IDeferrer deferrer);
        Task<IPvPPrefabContainer<PvPBackgroundImageStats>> GetBackgroundStatsAsync(int levelNum);
    }
}

