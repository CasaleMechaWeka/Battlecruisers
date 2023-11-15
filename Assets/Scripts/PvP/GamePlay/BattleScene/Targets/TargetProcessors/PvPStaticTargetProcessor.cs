using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPStaticTargetProcessor : IPvPTargetProcessor
    {
        private readonly IPvPTarget _target;

        public PvPStaticTargetProcessor(IPvPTarget target)
        {
            Assert.IsNotNull(target);
            _target = target;
        }

        public void AddTargetConsumer(IPvPTargetConsumer targetConsumer)
        {
            targetConsumer.Target = _target;
        }

        public void RemoveTargetConsumer(IPvPTargetConsumer targetConsumer) { }
        public void DisposeManagedState() { }
    }
}