using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.AI.TaskProducers
{
    public class AntiThreatTaskProducer : TaskProducer
    {
        private List<BuildingKey> _antiThreatBuildOrder;
        private readonly List<BuildingKey> _buildOrderOriginal;
        private readonly BaseThreatMonitor _threatMonitor;
        private readonly SlotNumCalculator _slotNumCalculator;

        private int _targetNumOfSlotsToUse;
        private int _numOfTasksCompleted;
        private IPrioritisedTask _currentTask;
        private LevelInfo _levelInfo;

        public AntiThreatTaskProducer(
            TaskList tasks,
            ICruiserController cruiser,
            ITaskFactory taskFactory,
            BuildingKey[] antiThreatBuildOrder,
            BaseThreatMonitor threatMonitor,
            SlotNumCalculator slotNumCalculator,
            LevelInfo levelInfo)
            : base(tasks, cruiser, taskFactory)
        {
            Helper.AssertIsNotNull(antiThreatBuildOrder, threatMonitor, slotNumCalculator);

            _antiThreatBuildOrder = antiThreatBuildOrder.ToList();
            _buildOrderOriginal = _antiThreatBuildOrder;
            _threatMonitor = threatMonitor;
            _slotNumCalculator = slotNumCalculator;

            _levelInfo = levelInfo;

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
                && _antiThreatBuildOrder.Count > 0)
            {
                BuildingKey buildingToConstruct = _levelInfo.CanConstructBuilding(_antiThreatBuildOrder[0]) ?
                                                  _antiThreatBuildOrder[0] :
                                                  _buildOrderOriginal[0];

                _currentTask = _taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, buildingToConstruct);
                _antiThreatBuildOrder.RemoveAt(0);
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
