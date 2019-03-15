using System;

namespace BattleCruisers.Buildables
{
    public enum DamagableState
    {
        Damagable, Invincible
    }

    public interface IHealthTracker
    {
        float Health { get; }
        float MaxHealth { get; }
        DamagableState DamagableState { set; }

        event EventHandler HealthChanged;
        event EventHandler HealthGone;

        /// <returns>True if the target was damaged, false otherwise (eg: due to being in invincible state).</returns>
        bool TakeDamage(float damageAmount);

        void Repair(float repairAmount);
    }
}