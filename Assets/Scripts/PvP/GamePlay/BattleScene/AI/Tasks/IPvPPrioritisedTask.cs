using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    public enum PvPTaskPriority
    {
        Low, Normal, High
    }

    public interface IPvPPrioritisedTask
    {
        PvPTaskPriority Priority { get; }

        event EventHandler<EventArgs> Completed;

        // Immediately emits Completed event if task is already completed.
        void Start();
        void Stop();
    }
}
