using BattleCruisers.Buildables;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Damage
{
    /// <summary>
    /// Keeps track of a damagable, and emits an event when that damagable's 
    /// health drops below a specified threshold.
    /// </summary>
    public class HealthThresholdMonitor : IHealthThresholdMonitor
    {
        private readonly IDamagable _damagable;
        private readonly float _threshold;
        private bool _wasAboveThreshold;

        private const float MIN_THRESHOLD = 0;
        private const float MAX_THRESHOLD = 1;

        public event EventHandler ThresholdReached;

        public HealthThresholdMonitor(IDamagable damagable, float thresholdProportion)
        {
            Assert.IsNotNull(damagable);
            Assert.IsTrue(thresholdProportion > MIN_THRESHOLD);
            Assert.IsTrue(thresholdProportion < MAX_THRESHOLD);
            Assert.AreEqual(damagable.MaxHealth, damagable.Health);

            _damagable = damagable;
            _threshold = thresholdProportion * damagable.MaxHealth;
            _wasAboveThreshold = true;

            _damagable.HealthChanged += _damagable_HealthChanged;
        }

        private void _damagable_HealthChanged(object sender, EventArgs e)
        {
            if (_wasAboveThreshold
                && _damagable.Health < _threshold)
            {
                ThresholdReached?.Invoke(this, EventArgs.Empty);
            }

            _wasAboveThreshold = _damagable.Health >= _threshold;
        }
    }
}