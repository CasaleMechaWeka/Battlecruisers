using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public class HealthTracker : IHealthTracker
    {
        public const float MIN_HEALTH = 1;

        public float MaxHealth { get; }
        public HealthTrackerState State { private get; set; }

        private float _health;
        public float Health
        {
            get { return _health; }
            private set
            {
                if (value >= MaxHealth)
                {
                    _health = MaxHealth;
                }
                else if (value <= 0)
                {
                    _health = 0;
                    
                    HealthGone?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    _health = value;
                }

                HealthChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler HealthChanged;
        public event EventHandler HealthGone;

        public HealthTracker(float maxHealth)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
            State = HealthTrackerState.Mutable;
        }

        public bool RemoveHealth(float amountToRemove)
        {
            Assert.IsTrue(amountToRemove > 0);

            if (Health > 0
                && State == HealthTrackerState.Mutable)
            {
                Health -= amountToRemove;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddHealth(float amountToAdd)
        {
            if (amountToAdd > 0
                && Health < MaxHealth
                && State == HealthTrackerState.Mutable)
            {
                Health += amountToAdd;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetMinHealth()
        {
            Health = MIN_HEALTH;
        }
    }
}