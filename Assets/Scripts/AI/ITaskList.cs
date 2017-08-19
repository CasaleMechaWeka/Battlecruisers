using System;
using BattleCruisers.AI.Tasks;

namespace BattleCruisers.AI
{
    public interface ITaskList
    {
        bool IsEmpty { get; }
        ITask HighestPriorityTask { get; }

        event EventHandler HighestPriorityTaskChanged;
        event EventHandler IsEmptyChanged;

        void Add(ITask taskToAdd);
        void Remove(ITask taskToRemove);
    }
}
