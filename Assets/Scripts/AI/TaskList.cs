using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.AI
{
    /// <summary>
    /// Keeps a list of tasks sorted by task priority.  The last item
    /// in the list has the highest priority. 
    /// </summary>
    public class TaskList : ITaskList
    {
        private readonly IList<ITask> _tasks;

        public bool IsEmpty { get { return _tasks.Count == 0; } }

        public event EventHandler HighestPriorityTaskChanged;

        public TaskList()
        {
            _tasks = new List<ITask>();
        }

        public void Add(ITask taskToAdd)
        {
            int insertionIndex = _tasks.Count;
            bool isHighestPriorityTask = true;

            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_tasks[i].Priority >= taskToAdd.Priority)
                {
                    insertionIndex = i + 1;
                    isHighestPriorityTask = false;
                    break;
                }
            }

            _tasks.Insert(insertionIndex, taskToAdd);

            if (isHighestPriorityTask)
            {
                if (HighestPriorityTaskChanged != null)
                {
                    HighestPriorityTaskChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Remove(ITask taskToRemove)
        {
            _tasks.Remove(taskToRemove);
        }
		
		public ITask GetHighestPriorityTask()
		{
            Assert.IsFalse(IsEmpty);
            return _tasks.Last();
		}
    }
}
