using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public class PvPButtonVisibilityFilters : IPvPButtonVisibilityFilters
    {
        public IPvPBroadcastingFilter<IPvPBuildable> BuildableButtonVisibilityFilter { get; }
        public IPvPBroadcastingFilter<BuildingCategory> CategoryButtonVisibilityFilter { get; }
        public IFilter<ITarget> ChooseTargetButtonVisiblityFilter { get; }
        public IFilter<ITarget> DeletButtonVisiblityFilter { get; }
        public IPvPBroadcastingFilter DismissButtonVisibilityFilter { get; }
        public IPvPBroadcastingFilter SpeedButtonsEnabledFilter { get; }

        public PvPButtonVisibilityFilters(
            IPvPBroadcastingFilter<IPvPBuildable> buildableButtonVisibilityFilter,
            IPvPBroadcastingFilter<BuildingCategory> categoryButtonVisibilityFilter,
            IFilter<ITarget> chooseTargetButtonVisiblityFilter,
            IFilter<ITarget> deletButtonVisiblityFilter,
            IPvPBroadcastingFilter backButtonVisibilityFilter,
            IPvPBroadcastingFilter speedButtonEnabledFilter)
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