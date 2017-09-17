using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public abstract class Target : MonoBehaviour, ITarget
    {
        public float maxHealth;

        public bool IsDestroyed { get { return Health == 0; } }
        public Faction Faction { get; protected set; }
        public GameObject GameObject { get { return gameObject; } }
        public abstract TargetType TargetType { get; }
        public virtual TargetValue TargetValue { get { return TargetValue.Low; } }
        public virtual Vector2 Velocity { get { return new Vector2(0, 0); } }
        public Vector2 Position { get { return gameObject.transform.position; } }

		// Seems to be an okay approximation (for cruisers at least)
		private const float DEFAULT_HEALTH_GAIN_PER_DRONE_S = 1;

        public event EventHandler<DestroyedEventArgs> Destroyed;
        public event EventHandler<HealthChangedEventArgs> HealthChanged;

        private bool IsFullHealth { get { return Health == maxHealth; } }

        protected List<TargetType> _attackCapabilities;
        public virtual List<TargetType> AttackCapabilities { get { return _attackCapabilities; } }

        private float _health;
        public float Health
        {
            get { return _health; }
            private set
            {
                if (value >= maxHealth)
                {
                    _health = maxHealth;
                    OnFullyRepaired();
                }
                else if (value <= 0)
                {
                    _health = 0;
                    OnHealthGone();
                }
                else
                {
                    _health = value;
                }

                if (HealthChanged != null)
                {
                    Logging.Log(Tags.TARGET, "HealthChanged  " + this);
                    HealthChanged.Invoke(this, new HealthChangedEventArgs(_health));
                }
            }
        }

        public IRepairCommand RepairCommand { get; private set; }

        public float HealthGainPerDroneS { get; protected set; }

        public virtual void StaticInitialise()
		{
			_health = maxHealth;
			_attackCapabilities = new List<TargetType>();
            RepairCommand = new RepairCommand(RepairCommandExecute, CanRepairCommandExecute, this);
            HealthGainPerDroneS = DEFAULT_HEALTH_GAIN_PER_DRONE_S;
		}

		protected virtual void OnFullyRepaired() { }

		protected virtual void OnHealthGone()
		{
			OnDestroyed();
			InvokeDestroyedEvent();
			InternalDestroy();
        }

        public void Destroy()
        {
            Assert.IsFalse(IsDestroyed, "Same target should not be destroyed more than once scrub :P");
            Health = 0;
		}

		protected virtual void InternalDestroy()
		{
			Destroy(gameObject);
		}

		protected virtual void OnDestroyed() { }

		protected void InvokeDestroyedEvent()
		{
			if (Destroyed != null)
			{
				Destroyed.Invoke(this, new DestroyedEventArgs(this));
			}
		}

		public void TakeDamage(float damageAmount)
		{
            bool wasFullHealth = IsFullHealth;

            // Guard against the rare case where a target takes damage after it has
            // been destroyed, in the same frame it was destroyed in.
            if (Health > 0)
            {
	            Health -= damageAmount;
	            OnTakeDamage();
			}

            if (wasFullHealth)
            {
                RepairCommand.EmitCanExecuteChanged();
            }
		}

		protected virtual void OnTakeDamage() { }

		protected void RepairCommandExecute(float repairAmount)
		{
			Assert.IsTrue(Health < maxHealth);
			Health += repairAmount;
			OnRepair();

            if (IsFullHealth)
            {
                RepairCommand.EmitCanExecuteChanged();
            }
		}

        protected virtual bool CanRepairCommandExecute()
        {
            return Health < maxHealth;
        }

		protected virtual void OnRepair() { }
	}
}
