using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public class PvPButtonVisibilityFilters : IPvPButtonVisibilityFilters
    {
        public IPvPBroadcastingFilter<IPvPBuildable> BuildableButtonVisibilityFilter { get; }
        public IPvPBroadcastingFilter<PvPBuildingCategory> CategoryButtonVisibilityFilter { get; }
        public IPvPFilter<IPvPTarget> ChooseTargetButtonVisiblityFilter { get; }
        public IPvPFilter<IPvPTarget> DeletButtonVisiblityFilter { get; }
        public IPvPBroadcastingFilter DismissButtonVisibilityFilter { get; }
        public IPvPBroadcastingFilter SpeedButtonsEnabledFilter { get; }

        public PvPButtonVisibilityFilters(
            IPvPBroadcastingFilter<IPvPBuildable> buildableButtonVisibilityFilter,
            IPvPBroadcastingFilter<PvPBuildingCategory> categoryButtonVisibilityFilter,
            IPvPFilter<IPvPTarget> chooseTargetButtonVisiblityFilter,
            IPvPFilter<IPvPTarget> deletButtonVisiblityFilter,
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