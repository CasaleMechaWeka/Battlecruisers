using BattleCruisers.Buildables;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialProvider
    {
		ISlotPermitter SlotPermitter { get; }
		IBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        IBroadcastingFilter<IBuildable> ShouldBuildingBeEnabledFilter { get; }
        IBuildingPermitter BuildingPermitter { get; }
        IUIManagerSettablePermissions UIManagerPermissions { get; }
        // FELIX  Rnemae to Permitter?
        BasicFilter IsNavigationEnabledFilter { get; }
        BasicFilter SpeedButtonsPermitter { get; }
        IUserChosenTargetHelperSettablePermissions UserChosenTargetPermissions { get; }

        IBuildSpeedController PlayerCruiserBuildSpeedController { get; }
        IBuildSpeedController AICruiserBuildSpeedController { get; }

        ISingleBuildableProviderNEW SingleAircraftProvider { get; }
		ISingleBuildableProviderNEW SingleShipProvider { get; }
        ISingleBuildableProviderNEW SingleOffensiveProvider { get; }
        ISingleBuildableProviderNEW CreateLastIncompleteBuildingStartedProvider(ICruiserController cruiser);
    }
}
