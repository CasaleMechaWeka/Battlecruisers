using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public interface IPvPBattleSceneHelper
    {
        IPrefabKey PlayerACruiser { get; }
        IPrefabKey PlayerBCruiser { get; }
        string[] PvPHullNames { get; }
        BuildingCategoryFilter BuildingCategoryPermitter { get; }
        IPvPLevel GetPvPLevel();
        ILoadout GetPlayerLoadout();
        IFilter<IPvPSlot> CreateHighlightableSlotFilter();
        IPvPBuildProgressCalculator CreatePlayerACruiserBuildProgressCalculator();
        IPvPBuildProgressCalculator CreatePlayerBCruiserBuildProgressCalculator();
        PvPUIManager CreateUIManager();
        void InitialiseUIManager(
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            IPvPBuildMenu buildMenu,
            IPvPItemDetailsManager detailsManager,
            IPrioritisedSoundPlayer soundPlayer,
            SingleSoundPlayer uiSoundPlayer);
        IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(DroneManager droneManager);
        IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(PvPCruiser playerCruiser);
        IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(IPvPCruiser playerCruiser);
        IUserChosenTargetHelper CreateUserChosenTargetHelper(
                             IUserChosenTargetManager playerCruiserUserChosenTargetManager
                             // IPrioritisedSoundPlayer soundPlayer,
                             //  ITargetIndicator targetIndicator
                             );
        IUserChosenTargetHelper CreateUserChosenTargetHelper(
            IUserChosenTargetManager playerCruiserUserChosenTargetManager,
            IPrioritisedSoundPlayer soundPlayer,
            ITargetIndicator targetIndicator
                                );
        IManagedDisposable CreateDroneEventSoundPlayer(IPvPCruiser playerCruiser, IDeferrer deferrer);
        Task<PrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum);
    }
}

