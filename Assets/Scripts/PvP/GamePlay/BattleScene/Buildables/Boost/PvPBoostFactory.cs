namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public class PvPBoostFactory : IPvPBoostFactory
    {
        private const float DEFAULT_BOOST_MULTIPLIER = 1;

        public IPvPBoostableGroup CreateBoostableGroup()
        {
            return new PvPBoostableGroup(this);
        }

        public IPvPBoostConsumer CreateBoostConsumer()
        {
            return new PvPBoostConsumer();
        }

        public IPvPBoostProvider CreateBoostProvider(float boostMultiplier)
        {
            return new PvPBoostProvider(boostMultiplier);
        }

        public IPvPBoostable CreateBoostable()
        {
            return new PvPBoostable(DEFAULT_BOOST_MULTIPLIER);
        }
    }
}
