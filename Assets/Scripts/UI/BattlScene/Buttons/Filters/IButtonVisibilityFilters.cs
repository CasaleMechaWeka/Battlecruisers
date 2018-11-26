using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    // NEWUI  Remove unused properties :)
    public interface IButtonVisibilityFilters
    {
        IBroadcastingFilter<IBuildable> BuildableButtonVisibilityFilter { get; }
        IBroadcastingFilter<BuildingCategory> CategoryButtonVisibilityFilter { get; }
        IFilter<ITarget> ChooseTargetButtonVisiblityFilter { get; }
        IFilter<ITarget> DeletButtonVisiblityFilter { get; }
        BasicFilter BackButtonVisibilityFilter { get; }
        IBroadcastingFilter SpeedButtonsEnabledFilter { get; }
    }
}