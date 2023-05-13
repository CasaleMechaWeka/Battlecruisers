using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public interface IPvPButtonVisibilityFilters
    {
        IPvPBroadcastingFilter<IPvPBuildable> BuildableButtonVisibilityFilter { get; }
        IPvPBroadcastingFilter<PvPBuildingCategory> CategoryButtonVisibilityFilter { get; }
        IPvPFilter<IPvPTarget> ChooseTargetButtonVisiblityFilter { get; }
        IPvPFilter<IPvPTarget> DeletButtonVisiblityFilter { get; }
        IPvPBroadcastingFilter DismissButtonVisibilityFilter { get; }
        IPvPBroadcastingFilter SpeedButtonsEnabledFilter { get; }
    }
}