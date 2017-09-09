using System.Collections.Generic;

namespace BattleCruisers.Buildables.Boost
{
    public class BoostProvider : IBoostProvider
    {
        private IList<IBoostUser> _boostUsers;

        public float BoostMultiplier { get; private set; }

        public BoostProvider(float boostMultiplier)
        {
            BoostMultiplier = boostMultiplier;
            _boostUsers = new List<IBoostUser>();
        }

        public void AddBoostConsumer(IBoostUser boostConsumer)
        {
            _boostUsers.Add(boostConsumer);
            boostConsumer.AddBoostProvider(this);
        }

        public void ClearBoostConsumers()
        {
            foreach (IBoostUser consumer in _boostUsers)
            {
                consumer.RemoveBoostProvider(this);
            }

            _boostUsers.Clear();
        }
    }
}
