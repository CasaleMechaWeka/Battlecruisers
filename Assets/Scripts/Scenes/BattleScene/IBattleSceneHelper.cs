using BattleCruisers.AI;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes.BattleScene
{
    public interface IBattleSceneHelper
    {
        IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; }
        IBuildProgressCalculator AICruiserBuildProgressCalculator { get; }
        IBuildingCategoryPermitter BuildingCategoryPermitter { get; }

        // Separate methods because of circular dependency between UIManager and everything else :/
        IUIManager CreateUIManager();
        void InitialiseUIManager(ManagerArgs args);

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
    }
}
