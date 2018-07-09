using BattleCruisers.AI.TaskProducers;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.AI
{
    // FELIX  Test :P

    /// <summary>
    /// 1. Ensures there is a reference to ITaskProducers, so the garbage
    ///     collector does not clean them up.
    /// 2. Provides a way to dispose all task producers, cleaning up their
    ///     event handlers.
    /// </summary>
    public class ArtificialIntelligence : IArtificialIntelligence
    {
        private readonly IList<ITaskProducer> _taskProducers;

        public ArtificialIntelligence(IList<ITaskProducer> taskProducers)
        {
            Assert.IsNotNull(taskProducers);
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
