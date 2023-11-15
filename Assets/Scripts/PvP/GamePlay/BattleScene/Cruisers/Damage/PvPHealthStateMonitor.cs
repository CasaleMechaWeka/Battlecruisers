using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage
{
    public class PvPHealthStateMonitor : IPvPHealthStateMonitor
    {
        private readonly PvPTarget _damagable;

        private PvPHealthState _healthState;
        public PvPHealthState HealthState
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
            _healthState = PvPHealthState.FullHealth;

            _damagable.pvp_Health.OnValueChanged += _damagable_HealthChanged;
            _damagable.pvp_Destroyed.OnValueChanged += _damagable_Destroyed;
        }

        private void _damagable_HealthChanged(float oldVal, float newVal)
        {
            float healthProportionRemaining = newVal / _damagable.MaxHealth;
            HealthState = FindHealthState(healthProportionRemaining);
        }

        private PvPHealthState FindHealthState(float healthProportionRemaining)
        {
            if (healthProportionRemaining == 1)
            {
                return PvPHealthState.FullHealth;
            }
            else if (healthProportionRemaining >= DAMAGED_THRESHOLD)
            {
                return PvPHealthState.SlightlyDamaged;
            }
            else if (healthProportionRemaining >= SEVERELY_DAMAGED_THRESHOLD)
            {
                return PvPHealthState.Damaged;
            }
            else
            {
                return PvPHealthState.SeverelyDamaged;
            }
        }

        private void _damagable_Destroyed(bool oldVal, bool newVal)
        {
            HealthState = PvPHealthState.NoHealth;

            _damagable.pvp_Health.OnValueChanged -= _damagable_HealthChanged;
            _damagable.pvp_Destroyed.OnValueChanged -= _damagable_Destroyed;
        }
    }
}