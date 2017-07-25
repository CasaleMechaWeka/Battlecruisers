using System;

namespace BattleCruisers.AI.Tasks
{
    public interface IInternalTask
    {
        event EventHandler Completed;

        void Start();
        void Stop();
        void Resume();
    }
}
