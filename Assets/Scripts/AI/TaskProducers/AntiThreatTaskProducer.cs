using System;
using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.TaskProducers
{
    public class AntiThreatTaskProducer : BaseTaskProducer
    {
        private readonly IDynamicBuildOrder _antiThreatBuildOrder;
        private readonly IThreatMonitor _threatMonitor;
        private readonly ISlotNumCalculator _slotNumCalculator;

        private int _targetNumOfSlotsToUse;
        private int _numOfTasksCompleted;
        private ITask _currentTask;
		
        public AntiThreatTaskProducer(ITaskList tasks, ICruiserController cruiser, IPrefabFactory prefabFactory, ITaskFactory taskFactory, 
            IDynamicBuildOrder antiThreatBuildOrder, IThreatMonitor threatMonitor, ISlotNumCalculator slotNumCalculator)
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
            Helper.AssertIsNotNull(antiThreatBuildOrder, threatMonitor, slotNumCalculator);

            _antiThreatBuildOrder = antiThreatBuildOrder;
            _threatMonitor = threatMonitor;
            _slotNumCalculator = slotNumCalculator;

            _targetNumOfSlotsToUse = 0;
            _numOfTasksCompleted = 0;

            _threatMonitor.ThreatLevelChanged += _threatMonitor_ThreatLevelChanged;
        }

        private void _threatMonitor_ThreatLevelChanged(object sender, EventArgs e)
        {
            _targetNumOfSlotsToUse = _slotNumCalculator.FindSlotNum(_threatMonitor.CurrentThreatLevel);

            CreateNextTask();
        }

		private void CreateNextTask()
        {
            if (_currentTask == null
                && _targetNumOfSlotsToUse > _numOfTasksCompleted
                && _antiThreatBuildOrder.MoveNext())
            {
                _currentTask = _taskFactory.CreateConstructBuildingTask(TaskPriority.High, _antiThreatBuildOrder.Current);
                _currentTask.Completed += _currentTask_Completed;
                _tasks.Add(_currentTask);
            }
        }
        private void _currentTask_Completed(object sender, EventArgs e)
        {
            _currentTask.Completed -= _currentTask_Completed;
            _numOfTasksCompleted++;
            _currentTask = null;

            CreateNextTask();
        }
    }
}
