using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Boost
{
    public class BoostableGroup : IBoostableGroup, IDisposable
	{
        private readonly IBoostConsumer _boostConsumer;
        private readonly IList<IBoostable> _boostables;

        public BoostableGroup(IBoostConsumer boostConsumer)
        {
            Assert.IsNotNull(boostConsumer);

            _boostConsumer = boostConsumer;
            _boostables = new List<IBoostable>();

            _boostConsumer.BoostChanged += _boostConsumer_BoostChanged;
        }

        private void _boostConsumer_BoostChanged(object sender, EventArgs e)
        {
            foreach (IBoostable boostable in _boostables)
            {
                boostable.BoostMultiplier = _boostConsumer.CumulativeBoost;
            }
        }

        public void AddBoostable(IBoostable boostable)
        {
            Assert.IsFalse(_boostables.Contains(boostable));

            _boostables.Add(boostable);
            boostable.BoostMultiplier = _boostConsumer.CumulativeBoost;
        }

        public void Dispose()
        {
            _boostConsumer.BoostChanged -= _boostConsumer_BoostChanged;
        }
    }
}
