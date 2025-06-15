using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;

namespace BattleCruisers.Scenes.BattleScene
{
    public interface IBattleSceneHelper
    {
        bool ShowInGameHints { get; }
        BuildingCategoryFilter BuildingCategoryPermitter { get; }
        IPrefabKey PlayerCruiser { get; }

        // Separate methods because of circular dependency between UIManager and everything else :/
        UIManager CreateUIManager();
        public abstract void InitialiseUIManager(ICruiser PlayerCruiser,
                                                 ICruiser AICruiser,
                                                 BuildMenu BuildMenu,
                                                 ItemDetailsManager DetailsManager,
                                                 IPrioritisedSoundPlayer SoundPlayer,
                                                 SingleSoundPlayer UISoundPlayer);

        IBuildProgressCalculator CreatePlayerCruiserBuildProgressCalculator();
        IBuildProgressCalculator CreateAICruiserBuildProgressCalculator();
        ILevel GetLevel();
        SideQuestData GetSideQuest();
        Loadout GetPlayerLoadout();
        IManagedDisposable CreateAI(Cruiser aiCruiser, Cruiser playerCruiser, int currentLevelNum);
        IFilter<ISlot> CreateHighlightableSlotFilter();
        ButtonVisibilityFilters CreateButtonVisibilityFilters(DroneManager droneManager);
        IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IDeferrer deferrer);
        IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser);
        IUserChosenTargetHelper CreateUserChosenTargetHelper(
            IUserChosenTargetManager playerCruiserUserChosenTargetManager,
            IPrioritisedSoundPlayer soundPlayer,
            TargetIndicatorController targetIndicator);
        Task<string> GetEnemyNameAsync(int levelNum);
        Task<PrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum);
        IPrefabKey GetAiCruiserKey();
    }
}
