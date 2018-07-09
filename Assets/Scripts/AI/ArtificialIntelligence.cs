using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.AI
{
    /// <summary>
    /// 1. Ensures there is a reference to the ITaskConsumer and ITaskProducers, 
    ///     so the garbage collector does not clean them up.
    /// 2. Provides a way to dispose all task producers, cleaning up their
    ///     event handlers.
    /// </summary>
    public class ArtificialIntelligence : IArtificialIntelligence
    {
        private readonly ITaskConsumer _taskConsumer;
        private readonly IList<ITaskProducer> _taskProducers;

        public ArtificialIntelligence(ITaskConsumer taskConsumer, IList<ITaskProducer> taskProducers)
        {
            Helper.AssertIsNotNull(taskConsumer, taskProducers);

            _taskConsumer = taskConsumer;
            _taskProducers = taskProducers;
        }

        public void DisposeManagedState()
        {
            foreach (ITaskProducer taskProducer in _taskProducers)
            {
                taskProducer.DisposeManagedState();
            }
        }
    }
}
