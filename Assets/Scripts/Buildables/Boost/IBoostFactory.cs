namespace BattleCruisers.Buildables.Boost
{
    public interface IBoostFactory
    {
        IBoostConsumer CreateBoostConsumer();
        IBoostProvider CreateBoostProvider();
        IBoostableGroup CreateBoostableGroup();
    }
}
