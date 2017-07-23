using System;

namespace BattleCruisers.AI
{
    public interface ITaskList
    {
        bool IsEmpty { get; }
        ITask HighestPriorityTask { get; }

        event EventHandler HighestPriorityTaskChanged;

        void Add(ITask taskToAdd);
        void Remove(ITask taskToRemove);
    }
}
