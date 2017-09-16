namespace BattleCruisers.Buildables.Boost
{
    public class BoostFactory : IBoostFactory
    {
        public IBoostableGroup CreateBoostableGroup()
        {
            return new BoostableGroup(this);
        }

        public IBoostConsumer CreateBoostConsumer()
        {
            return new BoostConsumer();
        }

        public IBoostProvider CreateBoostProvider(float boostMultiplier)
        {
            return new BoostProvider(boostMultiplier);
        }

        public IBoostable CreateBoostable()
        {
            return new Boostable();
        }
    }
}
