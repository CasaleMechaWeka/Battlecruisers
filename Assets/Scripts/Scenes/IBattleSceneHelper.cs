using BattleCruisers.AI;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes
{
    public interface IBattleSceneHelper
    {
        IUIManager CreateUIManager(IManagerArgs args);
        ILoadout GetPlayerLoadout();
        IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum);
        ISlotFilter CreateHighlightableSlotFilter();
        IButtonVisibilityFilters CreateButtonVisibilityFilters(IDroneManager droneManager);
        IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IVariableDelayDeferrer deferrer);
        IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser);

        IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; }
        IBuildProgressCalculator AICruiserBuildProgressCalculator { get; }
    }
}
