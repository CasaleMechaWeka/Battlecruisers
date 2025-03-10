using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public interface IPvPButtonVisibilityFilters
    {
        IBroadcastingFilter<IPvPBuildable> BuildableButtonVisibilityFilter { get; }
        IBroadcastingFilter<BuildingCategory> CategoryButtonVisibilityFilter { get; }
        IFilter<ITarget> ChooseTargetButtonVisiblityFilter { get; }
        IFilter<ITarget> DeletButtonVisiblityFilter { get; }
        IBroadcastingFilter DismissButtonVisibilityFilter { get; }
        IBroadcastingFilter SpeedButtonsEnabledFilter { get; }
    }
}