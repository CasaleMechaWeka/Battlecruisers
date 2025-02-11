using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public class PvPButtonVisibilityFilters : IPvPButtonVisibilityFilters
    {
        public IBroadcastingFilter<IPvPBuildable> BuildableButtonVisibilityFilter { get; }
        public IBroadcastingFilter<BuildingCategory> CategoryButtonVisibilityFilter { get; }
        public IFilter<ITarget> ChooseTargetButtonVisiblityFilter { get; }
        public IFilter<ITarget> DeletButtonVisiblityFilter { get; }
        public IBroadcastingFilter DismissButtonVisibilityFilter { get; }
        public IBroadcastingFilter SpeedButtonsEnabledFilter { get; }

        public PvPButtonVisibilityFilters(
            IBroadcastingFilter<IPvPBuildable> buildableButtonVisibilityFilter,
            IBroadcastingFilter<BuildingCategory> categoryButtonVisibilityFilter,
            IFilter<ITarget> chooseTargetButtonVisiblityFilter,
            IFilter<ITarget> deletButtonVisiblityFilter,
            IBroadcastingFilter backButtonVisibilityFilter,
            IBroadcastingFilter speedButtonEnabledFilter)
        {
            PvPHelper.AssertIsNotNull(
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