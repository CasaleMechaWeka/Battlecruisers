using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Manager
{
    public interface IManagerArgs
    {
        ICruiser PlayerCruiser { get; }
        ICruiser AICruiser { get; }
        IBuildMenu BuildMenu { get; }
        IBuildableDetailsManager DetailsManager { get; }
        IFilter<IBuilding> ShouldBuildingDeleteButtonBeEnabledFilter { get; }
    }
}
