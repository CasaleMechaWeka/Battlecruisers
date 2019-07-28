namespace BattleCruisers.Utils.BattleScene.Update
{
    public interface IUpdaterProvider
    {
        IUpdater PerFrameUpdater { get; }
        IUpdater PhysicsUpdater { get; }
        IUpdater SlowerUpdater { get; }
    }
}