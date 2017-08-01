using System;
using System.Collections.Generic;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.TaskProducers
{
    /// <summary>
    /// Responds to air threats.
    /// </summary>
    public class AntiAirTaskProducer : BaseTaskProducer
    {
        private readonly IList<IPrefabKey> _antiAirBuildOrder;
        private readonly IThreatMonitor _airThreatMonitor;
        private readonly ISlotNumCalculator _slotNumCalculator;

        private int _targetNumOfSlotsToUse;
        private int _numOfTasksCompleted;
        private ITask _currentTask;
		
        public AntiAirTaskProducer(ITaskList tasks, ICruiserController cruiser, IPrefabFactory prefabFactory, ITaskFactory taskFactory, 
            IList<IPrefabKey> antiAirBuildOrder, IThreatMonitor airThreatMonitor, ISlotNumCalculatorFactory slotNumCalculatorFactory)
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
            _antiAirBuildOrder = antiAirBuildOrder;
            _airThreatMonitor = airThreatMonitor;

            int maxNumOfDeckSlots = FindMaxNumOfDeckSlots(_cruiser.SlotWrapper.GetSlotCount(SlotType.Deck));
            _slotNumCalculator = slotNumCalculatorFactory.CreateAntiAirSlotNumCalculator(maxNumOfDeckSlots);

            _targetNumOfSlotsToUse = 0;
            _numOfTasksCompleted = 0;

            _airThreatMonitor.ThreatLevelChanged += _airThreatMonitor_ThreatLevelChanged;
        }

        /// <returns>Half, rounded up.</returns>
        private int FindMaxNumOfDeckSlots(int totalNumOfSlots)
        {
            return totalNumOfSlots / 2 + totalNumOfSlots % 2;
        }

        private void _airThreatMonitor_ThreatLevelChanged(object sender, EventArgs e)
        {
            _targetNumOfSlotsToUse = _slotNumCalculator.FindSlotNum(_airThreatMonitor.CurrentThreatLevel);

            CreateNextTask();
        }

		private void CreateNextTask()
        {
            if (_currentTask == null
                && _targetNumOfSlotsToUse > _numOfTasksCompleted)
            {
                Assert.IsTrue(_antiAirBuildOrder.Count > _numOfTasksCompleted);
                _currentTask = _taskFactory.CreateConstructBuildingTask(TaskPriority.High, _antiAirBuildOrder[_numOfTasksCompleted]);
                _currentTask.Completed += _currentTask_Completed;
                _tasks.Add(_currentTask);
            }
        }
        private void _currentTask_Completed(object sender, EventArgs e)
        {
            _currentTask.Completed -= _currentTask_Completed;
            _currentTask = null;

            CreateNextTask();
        }
    }
}
