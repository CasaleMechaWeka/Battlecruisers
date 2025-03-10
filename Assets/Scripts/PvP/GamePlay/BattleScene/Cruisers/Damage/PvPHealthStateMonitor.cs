using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage
{
    public class PvPHealthStateMonitor : IHealthStateMonitor
    {
        private readonly PvPTarget _damagable;

        private HealthState _healthState;
        public HealthState HealthState
        {
            get { return _healthState; }
            private set
            {
                if (_healthState != value)
                {
                    _healthState = value;

                    HealthStateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public const float DAMAGED_THRESHOLD = 0.67f;
        public const float SEVERELY_DAMAGED_THRESHOLD = 0.33f;

        public event EventHandler HealthStateChanged;

        public PvPHealthStateMonitor(PvPTarget damagable)
        {
            Assert.IsNotNull(damagable);

            _damagable = damagable;
            _healthState = HealthState.FullHealth;

            _damagable.pvp_Health.OnValueChanged += _damagable_HealthChanged;
            _damagable.pvp_Destroyed.OnValueChanged += _damagable_Destroyed;
        }

        private void _damagable_HealthChanged(float oldVal, float newVal)
        {
            float healthProportionRemaining = newVal / _damagable.MaxHealth;
            HealthState = FindHealthState(healthProportionRemaining);
        }

        private HealthState FindHealthState(float healthProportionRemaining)
        {
            if (healthProportionRemaining >= 1)
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

        private void _damagable_Destroyed(bool oldVal, bool newVal)
        {
            HealthState = HealthState.NoHealth;

            _damagable.pvp_Health.OnValueChanged -= _damagable_HealthChanged;
            _damagable.pvp_Destroyed.OnValueChanged -= _damagable_Destroyed;
        }
    }
}