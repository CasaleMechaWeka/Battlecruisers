using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails;

namespace BattleCruisers.UI.BattleScene
{
    public interface IBuildMenuCanvasController
    {
        IBuildableDetails<IBuilding> BuildingDetails { get; }
        IBuildableDetails<IUnit> UnitDetails { get; }
        ICruiserDetails CruiserDetails { get; }
        CruiserInfoController PlayerCruiserInfo { get; }
        CruiserInfoController AICruiserInfo { get; }
    }
}
