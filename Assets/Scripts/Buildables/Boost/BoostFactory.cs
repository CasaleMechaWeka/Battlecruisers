namespace BattleCruisers.Buildables.Boost
{
    public class BoostFactory : IBoostFactory
    {
        private const float DEFAULT_BOOST_MULTIPLIER = 1;

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
            return new Boostable(DEFAULT_BOOST_MULTIPLIER);
        }
    }
}
