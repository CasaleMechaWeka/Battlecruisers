using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetProcessors;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPStaticTargetProcessor : ITargetProcessor
    {
        private readonly ITarget _target;

        public PvPStaticTargetProcessor(ITarget target)
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