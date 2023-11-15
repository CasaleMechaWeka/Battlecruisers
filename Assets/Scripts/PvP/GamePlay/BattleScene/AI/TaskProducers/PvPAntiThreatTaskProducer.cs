using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers
{
    public class PvPAntiThreatTaskProducer : PvPTaskProducer
    {
        private readonly IPvPDynamicBuildOrder _antiThreatBuildOrder;
        private readonly IPvPThreatMonitor _threatMonitor;
        private readonly IPvPSlotNumCalculator _slotNumCalculator;

        private int _targetNumOfSlotsToUse;
        private int _numOfTasksCompleted;
        private IPvPPrioritisedTask _currentTask;

        public PvPAntiThreatTaskProducer(
            IPvPTaskList tasks,
            IPvPCruiserController cruiser,
            IPvPPrefabFactory prefabFactory,
            IPvPTaskFactory taskFactory,
            IPvPDynamicBuildOrder antiThreatBuildOrder,
            IPvPThreatMonitor threatMonitor,
            IPvPSlotNumCalculator slotNumCalculator)
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
            PvPHelper.AssertIsNotNull(antiThreatBuildOrder, threatMonitor, slotNumCalculator);

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
                _currentTask = _taskFactory.CreateConstructBuildingTask(PvPTaskPriority.Normal, _antiThreatBuildOrder.Current);
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
