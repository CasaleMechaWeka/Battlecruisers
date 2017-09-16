using System;

namespace BattleCruisers.Buildables.Boost
{
    public class BoostFactory : IBoostFactory
    {
        public IBoostableGroup CreateBoostableGroup()
        {
            throw new NotImplementedException();
        }

        public IBoostConsumer CreateBoostConsumer()
        {
            throw new NotImplementedException();
        }

        public IBoostProvider CreateBoostProvider()
        {
            throw new NotImplementedException();
        }
    }
}
