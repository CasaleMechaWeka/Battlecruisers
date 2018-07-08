using System;

namespace BattleCruisers.AI.Tasks
{
    // FELIX  Explain difference between this interface and ITask :/
    public interface IInternalTask
    {
        event EventHandler Completed;

        void Start();
        // FELIX  No need for Stop() and Resume() ???  Implementations are all empty...
        // But what if constructing building is stopped, but building completes => completed event, next task started...
        void Stop();
        void Resume();
    }
}
