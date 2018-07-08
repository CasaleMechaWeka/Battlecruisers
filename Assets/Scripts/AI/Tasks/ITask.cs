using System;

namespace BattleCruisers.AI.Tasks
{
    public interface ITask
    {
        event EventHandler Completed;

        void Start();
        
        // Currently Stop() and Resume() are not implemented anywhere, but could
        // come in handy if an AI task needs to perform some action to stop :)
        void Stop();
        void Resume();
    }
}
