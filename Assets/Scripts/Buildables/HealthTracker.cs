using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    // FELIX  Use :)
    // FELIX  Test :)
    public class HealthTracker : IHealthTracker
    {
        public float MaxHealth { get; private set; }
        public DamagableState DamagableState { private get; set; }

        private float _health;
        public float Health
        {
            get { return _health; }
            protected set
            {
                if (value >= MaxHealth)
                {
                    _health = MaxHealth;
                }
                else if (value <= 0)
                {
                    _health = 0;
                    
                    if (HealthGone != null)
                    {
                        HealthGone.Invoke(this, EventArgs.Empty);
                    }
                }
                else
                {
                    _health = value;
                }

                if (HealthChanged != null)
                {
                    HealthChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler HealthChanged;
        public event EventHandler HealthGone;

        public HealthTracker(float maxHealth)
        {
            MaxHealth = maxHealth;
        }

        public bool TakeDamage(float damageAmount)
        {
            Assert.IsTrue(damageAmount > 0);

            if (Health > 0
                && DamagableState == DamagableState.Damagable)
            {
                Health -= damageAmount;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Repair(float repairAmount)
        {
            Assert.IsTrue(repairAmount > 0);
            Health += repairAmount;
        }
    }
}