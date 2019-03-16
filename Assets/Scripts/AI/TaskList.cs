using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI
{
    /// <summary>
    /// Keeps a list of tasks sorted by task priority.  The last item
    /// in the list has the highest priority.
    /// 
    /// Not using List.Sort() because I will have many tasks with the
    /// same priority, and I want to maintain their order when I add
    /// or remove an task.
    /// </summary>
    public class TaskList : ITaskList
    {
        private readonly IList<IPrioritisedTask> _tasks;

        public bool IsEmpty { get { return _tasks.Count == 0; } }
		
        public IPrioritisedTask HighestPriorityTask { get { return _tasks.LastOrDefault(); } }

        public event EventHandler HighestPriorityTaskChanged;
        public event EventHandler IsEmptyChanged;

        public TaskList()
        {
            _tasks = new List<IPrioritisedTask>();
        }

        public void Add(IPrioritisedTask taskToAdd)
        {
            Logging.Log(Tags.AI, taskToAdd.ToString());
            Assert.IsFalse(_tasks.Contains(taskToAdd));

            bool wasEmpty = IsEmpty;
            int insertionIndex = _tasks.Count;

            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_tasks[i].Priority >= taskToAdd.Priority)
                {
                    insertionIndex = i;
                    break;
                }
            }

            _tasks.Insert(insertionIndex, taskToAdd);

            if (ReferenceEquals(taskToAdd, HighestPriorityTask))
            {
                Logging.Log(Tags.AI, "Added highest priority task, emit highest priority changed event");
                EmitHighestPriorityTaskChangedEvent();
            }

            if (wasEmpty)
            {
				EmitIsEmptyChangedEvent();
			}
        }

        public void Remove(IPrioritisedTask taskToRemove)
        {
            Logging.Log(Tags.AI, taskToRemove.ToString());
            Assert.IsTrue(_tasks.Contains(taskToRemove));

            bool wasHighestPriorityTask = ReferenceEquals(taskToRemove, HighestPriorityTask);

            _tasks.Remove(taskToRemove);

            if (wasHighestPriorityTask)
            {
                Logging.Log(Tags.AI, "Removed highest priority task, emit highest priority changed event");
                EmitHighestPriorityTaskChangedEvent();
            }

            if (IsEmpty)
            {
                EmitIsEmptyChangedEvent();
            }
        }

        private void EmitHighestPriorityTaskChangedEvent()
        {
            HighestPriorityTaskChanged?.Invoke(this, EventArgs.Empty);
        }

        private void EmitIsEmptyChangedEvent()
        {
            IsEmptyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
