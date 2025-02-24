using BattleCruisers.Buildables.Boost;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public class PvPBoostConsumer : IBoostConsumer
    {
        private readonly IList<IBoostProvider> _boostProviders;

        private const float DEFAULT_BOOST_MULTIPLIER = 1;

        private float _cumulativeBoost;
        public float CumulativeBoost
        {
            get { return _cumulativeBoost; }
            private set
            {
                if (_cumulativeBoost != value)
                {
                    _cumulativeBoost = value;

                    BoostChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler BoostChanged;

        public PvPBoostConsumer()
        {
            _cumulativeBoost = DEFAULT_BOOST_MULTIPLIER;
            _boostProviders = new List<IBoostProvider>();
        }

        public void AddBoostProvider(IBoostProvider boostProvider)
        {
            // Logging.LogMethod(Tags.BOOST);

            // Assert.IsFalse(_boostProviders.Contains(boostProvider));
            if (!_boostProviders.Contains(boostProvider))
            {
                _boostProviders.Add(boostProvider);
                UpdateCumulativeBoost();
            }
        }

        public void RemoveBoostProvider(IBoostProvider boostProvider)
        {
            // Logging.LogMethod(Tags.BOOST);

            //    Assert.IsTrue(_boostProviders.Contains(boostProvider));
            if (_boostProviders.Contains(boostProvider))
            {
                _boostProviders.Remove(boostProvider);
                UpdateCumulativeBoost();
            }
        }

        private void UpdateCumulativeBoost()
        {
            float cumulativeBoost = DEFAULT_BOOST_MULTIPLIER;

            foreach (IBoostProvider provider in _boostProviders)
            {
                cumulativeBoost *= provider.BoostMultiplier;
            }

            CumulativeBoost = cumulativeBoost;
        }
    }
}
