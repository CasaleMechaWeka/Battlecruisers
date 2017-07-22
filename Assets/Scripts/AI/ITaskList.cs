using System;

namespace BattleCruisers.AI
{
    public interface ITaskList
    {
        event EventHandler HighestPriorityTaskChanged;

        void Add(ITask task);
        void Remove(ITask task);
        ITask GetHighestPriorityTask();
    }
}
