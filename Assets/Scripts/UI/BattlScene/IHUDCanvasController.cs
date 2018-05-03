using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Common.BuildableDetails;

namespace BattleCruisers.UI.BattleScene
{
    public interface IHUDCanvasController
    {
        IBuildableDetails<IBuilding> BuildingDetails { get; }
        IBuildableDetails<IUnit> UnitDetails { get; }
        ICruiserDetails CruiserDetails { get; }

        CruiserInfoController PlayerCruiserInfo { get; }
        CruiserInfoController AICruiserInfo { get; }

        INavigationButtonsWrapper NavigationButtonsWrapper { get; }
        IGameSpeedWrapper GameSpeedWrapper { get; }
    }
}
