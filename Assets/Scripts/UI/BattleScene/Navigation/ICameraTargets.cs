using BattleCruisers.UI.Cameras.Targets;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface ICameraTargets
    {
        CameraTarget PlayerCruiserTarget { get; }
        CameraTarget PlayerCruiserDeathTarget { get; }
        CameraTarget PlayerCruiserNukedTarget { get; }
        CameraTarget PlayerNavalFactoryTarget { get; }
        CameraTarget EnemyCruiserTarget { get; }
        CameraTarget EnemyCruiserDeathTarget { get; }
        CameraTarget EnemyCruiserNukedTarget { get; }
        CameraTarget EnemyNavalFactoryTarget { get; }
        CameraTarget MidLeftTarget { get; }
        CameraTarget OverviewTarget { get; }
    }
}