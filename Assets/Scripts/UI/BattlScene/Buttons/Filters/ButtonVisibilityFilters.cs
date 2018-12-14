using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public class ButtonVisibilityFilters : IButtonVisibilityFilters
    {
        public IBroadcastingFilter<IBuildable> BuildableButtonVisibilityFilter  { get; private set; }
        public IBroadcastingFilter<BuildingCategory> CategoryButtonVisibilityFilter  { get; private set; }
        public IFilter<ITarget> ChooseTargetButtonVisiblityFilter  { get; private set; }
        public IFilter<ITarget> DeletButtonVisiblityFilter  { get; private set; }
        public BroadcastingFilter DismissButtonVisibilityFilter  { get; private set; }
        public IBroadcastingFilter SpeedButtonsEnabledFilter { get; private set; }

        public ButtonVisibilityFilters(
            IBroadcastingFilter<IBuildable> buildableButtonVisibilityFilter,
            IBroadcastingFilter<BuildingCategory> categoryButtonVisibilityFilter,
            IFilter<ITarget> chooseTargetButtonVisiblityFilter,
            IFilter<ITarget> deletButtonVisiblityFilter,
            BroadcastingFilter backButtonVisibilityFilter,
            IBroadcastingFilter speedButtonEnabledFilter)
        {
            Helper.AssertIsNotNull(
                buildableButtonVisibilityFilter, 
                categoryButtonVisibilityFilter, 
                chooseTargetButtonVisiblityFilter, 
                deletButtonVisiblityFilter, 
                backButtonVisibilityFilter,
                speedButtonEnabledFilter);

            BuildableButtonVisibilityFilter = buildableButtonVisibilityFilter;
            CategoryButtonVisibilityFilter = categoryButtonVisibilityFilter;
            ChooseTargetButtonVisiblityFilter = chooseTargetButtonVisiblityFilter;
            DeletButtonVisiblityFilter = deletButtonVisiblityFilter;
            DismissButtonVisibilityFilter = backButtonVisibilityFilter;
            SpeedButtonsEnabledFilter = speedButtonEnabledFilter;
        }
    }
}