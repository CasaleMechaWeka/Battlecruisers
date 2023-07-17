using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers;
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
    public class PvPArtificialIntelligence : IPvPArtificialIntelligence
    {
        private readonly PvPTaskConsumer _taskConsumer;
        private readonly IList<IPvPTaskProducer> _taskProducers;
        private readonly PvPDroneConsumerFocusManager _focusManager;

        public PvPArtificialIntelligence(PvPTaskConsumer taskConsumer, IList<IPvPTaskProducer> taskProducers, PvPDroneConsumerFocusManager focusManager)
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

            foreach (IPvPTaskProducer taskProducer in _taskProducers)
            {
                taskProducer.DisposeManagedState();
            }
        }
    }
}
