using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public class ButtonVisibilityFilters : IButtonVisibilityFilters
    {
        public IBroadcastingFilter<IBuildable> BuildableButtonVisibilityFilter  { get; }
        public IBroadcastingFilter<BuildingCategory> CategoryButtonVisibilityFilter  { get; }
        public IFilter<ITarget> ChooseTargetButtonVisiblityFilter  { get; }
        public IFilter<ITarget> DeletButtonVisiblityFilter  { get; }
        public IBroadcastingFilter DismissButtonVisibilityFilter  { get; }
        public IBroadcastingFilter SpeedButtonsEnabledFilter { get; }
        public BroadcastingFilter HelpLabelsVisibilityFilter { get; }

        public ButtonVisibilityFilters(
            IBroadcastingFilter<IBuildable> buildableButtonVisibilityFilter,
            IBroadcastingFilter<BuildingCategory> categoryButtonVisibilityFilter,
            IFilter<ITarget> chooseTargetButtonVisiblityFilter,
            IFilter<ITarget> deletButtonVisiblityFilter,
            IBroadcastingFilter backButtonVisibilityFilter,
            IBroadcastingFilter speedButtonEnabledFilter,
            BroadcastingFilter helpLabelsVisibilityFilter)
        {
            Helper.AssertIsNotNull(
                buildableButtonVisibilityFilter, 
                categoryButtonVisibilityFilter, 
                chooseTargetButtonVisiblityFilter, 
                deletButtonVisiblityFilter, 
                backButtonVisibilityFilter,
                speedButtonEnabledFilter,
                helpLabelsVisibilityFilter);

            BuildableButtonVisibilityFilter = buildableButtonVisibilityFilter;
            CategoryButtonVisibilityFilter = categoryButtonVisibilityFilter;
            ChooseTargetButtonVisiblityFilter = chooseTargetButtonVisiblityFilter;
            DeletButtonVisiblityFilter = deletButtonVisiblityFilter;
            DismissButtonVisibilityFilter = backButtonVisibilityFilter;
            SpeedButtonsEnabledFilter = speedButtonEnabledFilter;
            HelpLabelsVisibilityFilter = helpLabelsVisibilityFilter;
        }
    }
}