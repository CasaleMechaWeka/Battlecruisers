using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Boost
{
    public class BoostableGroup : IBoostableGroup, IDisposable
	{
        private readonly IList<IBoostable> _boostables;

		public IBoostConsumer BoostConsumer { get; private set; }

        public BoostableGroup(IBoostConsumer boostConsumer)
        {
            Assert.IsNotNull(boostConsumer);

            BoostConsumer = boostConsumer;
            _boostables = new List<IBoostable>();

            BoostConsumer.BoostChanged += _boostConsumer_BoostChanged;
        }

        private void _boostConsumer_BoostChanged(object sender, EventArgs e)
        {
            foreach (IBoostable boostable in _boostables)
            {
                boostable.BoostMultiplier = BoostConsumer.CumulativeBoost;
            }
        }

        public void AddBoostable(IBoostable boostable)
        {
            Assert.IsFalse(_boostables.Contains(boostable));

            _boostables.Add(boostable);
            boostable.BoostMultiplier = BoostConsumer.CumulativeBoost;
        }

        public void Dispose()
        {
            BoostConsumer.BoostChanged -= _boostConsumer_BoostChanged;
        }
    }
}
