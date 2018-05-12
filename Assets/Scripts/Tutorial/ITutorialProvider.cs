using BattleCruisers.Buildables;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons.Filters;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialProvider
    {
		ISlotPermitter SlotPermitter { get; }
		IBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        IFilter<IBuildable> ShouldBuildingBeEnabledFilter { get; }
        IBuildingPermitter BuildingPermitter { get; }
        BasicFilter NavigationPermitter { get; }

        ISingleBuildableProvider SingleAircraftProvider { get; }
		ISingleBuildableProvider SingleShipProvider { get; }

        IBuildSpeedController PlayerCruiserBuildSpeedController { get; }
        IBuildSpeedController AICruiserBuildSpeedController { get; }

        ISingleBuildableProvider CreateLastIncompleteBuildingStartedProvider(ICruiserController cruiser);
    }
}
