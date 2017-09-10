using System.Collections.Generic;

namespace BattleCruisers.Buildables.Boost
{
    public class BoostProvider : IBoostProvider
    {
        private IList<IBoostConsumer> _boostConsumers;

        public float BoostMultiplier { get; private set; }

        public BoostProvider(float boostMultiplier)
        {
            BoostMultiplier = boostMultiplier;
            _boostConsumers = new List<IBoostConsumer>();
        }

        public void AddBoostConsumer(IBoostConsumer boostConsumer)
        {
            _boostConsumers.Add(boostConsumer);
            boostConsumer.AddBoostProvider(this);
        }

        public void ClearBoostConsumers()
        {
            foreach (IBoostConsumer consumer in _boostConsumers)
            {
                consumer.RemoveBoostProvider(this);
            }

            _boostConsumers.Clear();
        }
    }
}
