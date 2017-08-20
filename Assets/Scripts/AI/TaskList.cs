using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.Tasks;
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
        private readonly IList<ITask> _tasks;

        public bool IsEmpty { get { return _tasks.Count == 0; } }
		
        public ITask HighestPriorityTask { get { return _tasks.LastOrDefault(); } }

        public event EventHandler HighestPriorityTaskChanged;
        public event EventHandler IsEmptyChanged;

        public TaskList()
        {
            _tasks = new List<ITask>();
        }

        public void Add(ITask taskToAdd)
        {
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
                EmitHighestPriorityTaskChangedEvent();
            }

            if (wasEmpty)
            {
				EmitIsEmptyChangedEvent();
			}
        }

        public void Remove(ITask taskToRemove)
        {
            Assert.IsTrue(_tasks.Contains(taskToRemove));

            bool wasHighestPriorityTask = ReferenceEquals(taskToRemove, HighestPriorityTask);

            _tasks.Remove(taskToRemove);

            if (wasHighestPriorityTask)
            {
                EmitHighestPriorityTaskChangedEvent();
            }

            if (IsEmpty)
            {
                EmitIsEmptyChangedEvent();
            }
        }

        private void EmitHighestPriorityTaskChangedEvent()
        {
            if (HighestPriorityTaskChanged != null)
            {
                HighestPriorityTaskChanged.Invoke(this, EventArgs.Empty);
            }
        }

        private void EmitIsEmptyChangedEvent()
        {
            if (IsEmptyChanged != null)
            {
				IsEmptyChanged.Invoke(this, EventArgs.Empty);
			}
        }
    }
}
