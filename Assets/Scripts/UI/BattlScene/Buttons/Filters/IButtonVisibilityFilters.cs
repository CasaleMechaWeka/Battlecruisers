using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public interface IButtonVisibilityFilters
    {
        IBroadcastingFilter<IBuildable> BuildableButtonVisibilityFilter { get; }
        IBroadcastingFilter<BuildingCategory> CategoryButtonVisibilityFilter { get; }
        IFilter<ITarget> ChooseTargetButtonVisiblityFilter { get; }
        IFilter<ITarget> DeletButtonVisiblityFilter { get; }
        BroadcastingFilter DismissButtonVisibilityFilter { get; }
        IBroadcastingFilter SpeedButtonsEnabledFilter { get; }
    }
}