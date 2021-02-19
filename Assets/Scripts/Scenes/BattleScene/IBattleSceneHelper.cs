using BattleCruisers.AI;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.BattleScene.Manager;
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
        IBuildingCategoryPermitter BuildingCategoryPermitter { get; }

        // Separate methods because of circular dependency between UIManager and everything else :/
        IUIManager CreateUIManager();
        void InitialiseUIManager(ManagerArgs args);

        IBuildProgressCalculator CreatePlayerCruiserBuildProgressCalculator();
        IBuildProgressCalculator CreateAICruiserBuildProgressCalculator();
        ILevel GetLevel();
        ILoadout GetPlayerLoadout();
        IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum);
        ISlotFilter CreateHighlightableSlotFilter();
        IButtonVisibilityFilters CreateButtonVisibilityFilters(IDroneManager droneManager);
        IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IDeferrer deferrer);
        IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser);
        IUserChosenTargetHelper CreateUserChosenTargetHelper(
            IUserChosenTargetManager playerCruiserUserChosenTargetManager, 
            IPrioritisedSoundPlayer soundPlayer,
            ITargetIndicator targetIndicator);
        Task<string> GetEnemyNameAsync(int levelNum);
        Task<IPrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum);
        IPrefabKey GetAiCruiserKey();
    }
}
