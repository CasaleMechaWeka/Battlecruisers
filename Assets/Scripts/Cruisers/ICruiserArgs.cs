using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserArgs
    {
        Faction Faction { get; }
        ICruiser EnemyCruiser { get; }
        IUIManager UiManager { get; }
        IDroneManager DroneManager { get; }
        IDroneConsumerProvider DroneConsumerProvider { get; }
        IFactoryProvider FactoryProvider { get; }
        Direction FacingDirection { get; }
        RepairManager RepairManager { get; }
        bool ShouldShowFog { get; }
        ICameraController CameraController { get; }
    }
}
