using System;

namespace BattleCruisers.Buildables
{
    public enum HealthTrackerState
    {
        Mutable, Immutable
    }

    public interface IHealthTracker
    {
        float Health { get; }
        float MaxHealth { get; }
        HealthTrackerState State { set; }

        event EventHandler HealthChanged;
        event EventHandler HealthGone;

        /// <returns>True if the health was removed, false otherwise.</returns>
        bool RemoveHealth(float amountToRemove);

        /// <returns>True if the health was added, false otherwise.</returns>
        bool AddHealth(float amountToAdd);

        void SetMinHealth();
        void SetHealth(float amount);
        
    }
}