using BattleCruisers.AI.Drones;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.AI
{
    /// <summary>
    /// 1. Ensures there is a reference to the ITaskConsumer and ITaskProducers, 
    ///     so the garbage collector does not clean them up.
    /// 2. Provides a way to dispose the ITaskConsumer and all ITaskProducers, 
    ///     cleaning up their event handlers.
    /// </summary>
    public class ArtificialIntelligence : IArtificialIntelligence
    {
        private readonly TaskConsumer _taskConsumer;
        private readonly IList<ITaskProducer> _taskProducers;
        private readonly DroneConsumerFocusManager _focusManager;

        public ArtificialIntelligence(TaskConsumer taskConsumer, IList<ITaskProducer> taskProducers, DroneConsumerFocusManager focusManager)
        {
            Helper.AssertIsNotNull(taskConsumer, taskProducers, focusManager);

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
