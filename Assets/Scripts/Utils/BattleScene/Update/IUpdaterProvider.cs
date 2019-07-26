namespace BattleCruisers.Utils.BattleScene.Update
{
    public interface IUpdaterProvider
    {
        IUpdater SlowerUpdater { get; }
    }
}