using BattleCruisers.AI;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes.BattleScene
{
    public interface IBattleSceneHelper
    {
        // Separate methods because of circular dependency between UIManager and everything else :/
        IUIManager CreateUIManager();
        void InitialiseUIManager(ManagerArgsNEW args);

        ILoadout GetPlayerLoadout();
        IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum);
        ISlotFilter CreateHighlightableSlotFilter();
        IButtonVisibilityFilters CreateButtonVisibilityFilters(IDroneManager droneManager);
        IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IVariableDelayDeferrer deferrer);
        IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser);
        IBroadcastingFilter CreateNavigationWheelEnabledFilter();

        IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; }
        IBuildProgressCalculator AICruiserBuildProgressCalculator { get; }
    }
}
