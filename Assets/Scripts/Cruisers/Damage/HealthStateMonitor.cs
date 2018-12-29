using BattleCruisers.Buildables;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Damage
{
    public class HealthStateMonitor : IHealthStateMonitor
    {
        private readonly IDamagable _damagable;

        private HealthState _healthState;
        public HealthState HealthState
        {
            get { return _healthState; }
            private set
            {
                if (_healthState != value)
                {
                    _healthState = value;

                    if (HealthStateChanged != null)
                    {
                        HealthStateChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public const float DAMAGED_THRESHOLD = 0.67f;
        public const float SEVERELY_DAMAGED_THRESHOLD = 0.33f;

        public event EventHandler HealthStateChanged;

        public HealthStateMonitor(IDamagable damagable)
        {
            Assert.IsNotNull(damagable);

            _damagable = damagable;
            _healthState = HealthState.FullHealth;

            _damagable.HealthChanged += _damagable_HealthChanged;
            _damagable.Destroyed += _damagable_Destroyed;
        }

        private void _damagable_HealthChanged(object sender, EventArgs e)
        {
            float healthProportionRemaining = _damagable.Health / _damagable.MaxHealth;
            HealthState = FindHealthState(healthProportionRemaining);
        }

        private HealthState FindHealthState(float healthProportionRemaining)
        {
            if (healthProportionRemaining == 1)
            {
                return HealthState.FullHealth;
            }
            else if (healthProportionRemaining >= DAMAGED_THRESHOLD)
            {
                return HealthState.SlightlyDamaged;
            }
            else if (healthProportionRemaining >= SEVERELY_DAMAGED_THRESHOLD)
            {
                return HealthState.Damaged;
            }
            else
            {
                return HealthState.SeverelyDamaged;
            }
        }

        private void _damagable_Destroyed(object sender, DestroyedEventArgs e)
        {
            HealthState = HealthState.NoHealth;

            _damagable.HealthChanged -= _damagable_HealthChanged;
            _damagable.Destroyed -= _damagable_Destroyed;
        }
    }
}