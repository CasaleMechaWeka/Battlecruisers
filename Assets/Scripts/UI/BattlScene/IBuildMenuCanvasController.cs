using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails;

namespace BattleCruisers.UI.BattleScene
{
    public interface IBuildMenuCanvasController
    {
        BuildingDetailsController BuildingDetails { get; }
        UnitDetailsController UnitDetails { get; }
        CruiserDetailsController CruiserDetails { get; }
        HealthBarController PlayerCruiserHealthBar { get; }
        HealthBarController AiCruiserHealthBar { get; }
    }
}
