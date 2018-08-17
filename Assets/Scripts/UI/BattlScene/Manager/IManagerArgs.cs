using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Filters;

namespace BattleCruisers.UI.BattleScene.Manager
{
    public interface IManagerArgs
    {
        ICruiser PlayerCruiser { get; }
        ICruiser AICruiser { get; }
        IBuildMenu BuildMenu { get; }
        IBuildableDetailsManager DetailsManager { get; }
        IBroadcastingFilter<IBuilding> ShouldBuildingDeleteButtonBeEnabledFilter { get; }
    }
}
