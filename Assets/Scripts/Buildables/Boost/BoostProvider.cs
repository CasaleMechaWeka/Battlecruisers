using System.Collections.Generic;
using UnityEngine.Assertions;

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
		
		public void RemoveBoostConsumer(IBoostConsumer boostConsumer)
		{
            Assert.IsTrue(_boostConsumers.Contains(boostConsumer));

            _boostConsumers.Remove(boostConsumer);
            boostConsumer.RemoveBoostProvider(this);
		}
    }
}
