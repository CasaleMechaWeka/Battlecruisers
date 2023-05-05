namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public interface IPvPUpdaterProvider
    {
        IPvPUpdater PerFrameUpdater { get; }
        IPvPUpdater PhysicsUpdater { get; }
        IPvPSwitchableUpdater SwitchableUpdater { get; }
        IPvPUpdater SlowUpdater { get; }
        IPvPUpdater VerySlowUpdater { get; }
        IPvPUpdater BarrelControllerUpdater { get; }
    }
}