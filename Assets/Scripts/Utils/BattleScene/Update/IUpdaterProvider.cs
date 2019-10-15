namespace BattleCruisers.Utils.BattleScene.Update
{
    public interface IUpdaterProvider
    {
        IUpdater PerFrameUpdater { get; }
        IUpdater PhysicsUpdater { get; }
        ISwitchableUpdater SwitchableUpdater { get; }
        IUpdater SlowerUpdater { get; }
        IUpdater BarrelControllerUpdater { get; }
    }
}