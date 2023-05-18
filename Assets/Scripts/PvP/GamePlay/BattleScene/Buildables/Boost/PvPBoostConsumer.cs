using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public class PvPBoostConsumer : IPvPBoostConsumer
    {
        private readonly IList<IPvPBoostProvider> _boostProviders;

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
            _boostProviders = new List<IPvPBoostProvider>();
        }

        public void AddBoostProvider(IPvPBoostProvider boostProvider)
        {
            // Logging.LogMethod(Tags.BOOST);

            Assert.IsFalse(_boostProviders.Contains(boostProvider));
            _boostProviders.Add(boostProvider);
            UpdateCumulativeBoost();
        }

        public void RemoveBoostProvider(IPvPBoostProvider boostProvider)
        {
            // Logging.LogMethod(Tags.BOOST);

            Assert.IsTrue(_boostProviders.Contains(boostProvider));
            _boostProviders.Remove(boostProvider);
            UpdateCumulativeBoost();
        }

        private void UpdateCumulativeBoost()
        {
            float cumulativeBoost = DEFAULT_BOOST_MULTIPLIER;

            foreach (IPvPBoostProvider provider in _boostProviders)
            {
                cumulativeBoost *= provider.BoostMultiplier;
            }

            CumulativeBoost = cumulativeBoost;
        }
    }
}
