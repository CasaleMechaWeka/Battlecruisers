using BattleCruisers.AI;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    /// <summary>
    /// 1. Ensures there is a reference to the ITaskConsumer and ITaskProducers, 
    ///     so the garbage collector does not clean them up.
    /// 2. Provides a way to dispose the ITaskConsumer and all ITaskProducers, 
    ///     cleaning up their event handlers.
    /// </summary>
    public class PvPArtificialIntelligence : IArtificialIntelligence
    {
        private readonly TaskConsumer _taskConsumer;
        private readonly IList<ITaskProducer> _taskProducers;
        private readonly PvPDroneConsumerFocusManager _focusManager;

        public PvPArtificialIntelligence(TaskConsumer taskConsumer, IList<ITaskProducer> taskProducers, PvPDroneConsumerFocusManager focusManager)
        {
            PvPHelper.AssertIsNotNull(taskConsumer, taskProducers, focusManager);

            _taskConsumer = taskConsumer;
            _taskProducers = taskProducers;
            _focusManager = focusManager;
        }

        public void DisposeManagedState()
        {
            _taskConsumer.DisposeManagedState();
            _focusManager.DisposeManagedState();

            foreach (ITaskProducer taskProducer in _taskProducers)
            {
                taskProducer.DisposeManagedState();
            }
        }
    }
}
