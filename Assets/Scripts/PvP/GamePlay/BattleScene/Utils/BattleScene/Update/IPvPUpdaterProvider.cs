using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public interface IPvPUpdaterProvider
    {
        IUpdater PerFrameUpdater { get; }
        IUpdater PhysicsUpdater { get; }
        ISwitchableUpdater SwitchableUpdater { get; }
        IUpdater SlowUpdater { get; }
        IUpdater VerySlowUpdater { get; }
        IUpdater BarrelControllerUpdater { get; }
    }
}