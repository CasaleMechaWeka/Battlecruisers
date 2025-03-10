using System;
using BattleCruisers.AI.Tasks;

namespace BattleCruisers.AI
{
    public interface ITaskList
    {
        bool IsEmpty { get; }
        IPrioritisedTask HighestPriorityTask { get; }

        event EventHandler HighestPriorityTaskChanged;
        event EventHandler IsEmptyChanged;

        void Add(IPrioritisedTask taskToAdd);
        void Remove(IPrioritisedTask taskToRemove);
    }
}
