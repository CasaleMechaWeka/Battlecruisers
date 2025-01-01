using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public interface IPvPButtonVisibilityFilters
    {
        IPvPBroadcastingFilter<IPvPBuildable> BuildableButtonVisibilityFilter { get; }
        IPvPBroadcastingFilter<BuildingCategory> CategoryButtonVisibilityFilter { get; }
        IFilter<IPvPTarget> ChooseTargetButtonVisiblityFilter { get; }
        IFilter<IPvPTarget> DeletButtonVisiblityFilter { get; }
        IPvPBroadcastingFilter DismissButtonVisibilityFilter { get; }
        IPvPBroadcastingFilter SpeedButtonsEnabledFilter { get; }
    }
}