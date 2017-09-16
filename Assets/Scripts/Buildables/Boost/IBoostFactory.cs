namespace BattleCruisers.Buildables.Boost
{
    public interface IBoostFactory
    {
        IBoostConsumer CreateBoostConsumer();
        IBoostProvider CreateBoostProvider(float boostMultiplier);
        IBoostableGroup CreateBoostableGroup();
    }
}
