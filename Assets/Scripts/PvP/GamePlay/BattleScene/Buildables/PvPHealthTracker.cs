using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public class PvPHealthTracker : IPvPHealthTracker
    {
        public const float MIN_HEALTH = 1;

        public float MaxHealth { get; }
        public PvPHealthTrackerState State { private get; set; }

        private float _health;
        private PvPTarget _playerCruiser;
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
                if (_playerCruiser != null)
                    _playerCruiser.pvp_Health.Value = _health;
            }
        }

        public event EventHandler HealthChanged;
        public event EventHandler HealthGone;

        public PvPHealthTracker(PvPTarget playerCruiser, float maxHealth)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
            State = PvPHealthTrackerState.Mutable;
            _playerCruiser = playerCruiser;
        }

        public bool RemoveHealth(float amountToRemove)
        {
            if (amountToRemove > 0
                && Health > 0
                && State == PvPHealthTrackerState.Mutable)
            {
                Health -= amountToRemove;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetHealth(float amount)
        {
            Health = amount;
        }

        public bool AddHealth(float amountToAdd)
        {
            if (amountToAdd > 0
                && Health < MaxHealth
                && State == PvPHealthTrackerState.Mutable)
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

        public void SetMaxHealth()
        {
            Health = MaxHealth;
        }
    }
}

