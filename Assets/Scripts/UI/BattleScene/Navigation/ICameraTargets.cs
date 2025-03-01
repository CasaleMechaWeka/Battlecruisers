using BattleCruisers.UI.Cameras.Targets;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface ICameraTargets
    {
        ICameraTarget PlayerCruiserTarget { get; }
        ICameraTarget PlayerCruiserDeathTarget { get; }
        ICameraTarget PlayerCruiserNukedTarget { get; }
        ICameraTarget PlayerNavalFactoryTarget { get; }

        ICameraTarget EnemyCruiserTarget { get; }
        ICameraTarget EnemyCruiserDeathTarget { get; }
        ICameraTarget EnemyCruiserNukedTarget { get; }
        ICameraTarget EnemyNavalFactoryTarget { get; }

        ICameraTarget MidLeftTarget { get; }
        ICameraTarget OverviewTarget { get; }
    }
}