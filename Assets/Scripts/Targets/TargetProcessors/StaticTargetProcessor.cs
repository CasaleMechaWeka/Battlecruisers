using BattleCruisers.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class StaticTargetProcessor : ITargetProcessor
    {
        private readonly ITarget _target;

        public StaticTargetProcessor(ITarget target)
        {
            Assert.IsNotNull(target);
            _target = target;
        }

        public void AddTargetConsumer(ITargetConsumer targetConsumer)
        {
            targetConsumer.Target = _target;
        }

        public void RemoveTargetConsumer(ITargetConsumer targetConsumer) { }
        public void DisposeManagedState() { }
    }
}