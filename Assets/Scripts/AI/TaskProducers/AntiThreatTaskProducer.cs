using System;
using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.TaskProducers
{
    public class AntiThreatTaskProducer : TaskProducer
    {
        private readonly IDynamicBuildOrder _antiThreatBuildOrder;
        private readonly BaseThreatMonitor _threatMonitor;
        private readonly ISlotNumCalculator _slotNumCalculator;

        private int _targetNumOfSlotsToUse;
        private int _numOfTasksCompleted;
        private IPrioritisedTask _currentTask;

        public AntiThreatTaskProducer(
            TaskList tasks,
            ICruiserController cruiser,
            ITaskFactory taskFactory,
            IDynamicBuildOrder antiThreatBuildOrder,
            BaseThreatMonitor threatMonitor,
            ISlotNumCalculator slotNumCalculator)
            : base(tasks, cruiser, taskFactory)
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
                _currentTask = _taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, _antiThreatBuildOrder.Current);
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

        public override void DisposeManagedState()
        {
            _threatMonitor.ThreatLevelChanged -= _threatMonitor_ThreatLevelChanged;
            base.DisposeManagedState();
        }
    }
}
