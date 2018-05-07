using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders;

namespace BattleCruisers.Tutorial
{
    public interface IPermitterProvider
    {
		ISlotPermitter SlotPermitter { get; }
		IBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        IFilter<IBuildable> BuildingActivenessDecider { get; }
        IBuildingPermitter BuildingPermitter { get; }
        BasicDecider NavigationPermitter { get; }
        ISingleBuildableProvider SingleAircraftProvider { get; }
		ISingleBuildableProvider SingleShipProvider { get; }

        ILastBuildingStartedProvider CreateLastBuildingStartedProvider(ICruiserController cruiser);
    }
}
