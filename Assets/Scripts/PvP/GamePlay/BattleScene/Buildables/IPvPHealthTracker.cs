using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public enum PvPHealthTrackerState
    {
        Mutable, Immutable
    }

    public interface IPvPHealthTracker
    {
        float Health { get; }
        float MaxHealth { get; }
        PvPHealthTrackerState State { set; }

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