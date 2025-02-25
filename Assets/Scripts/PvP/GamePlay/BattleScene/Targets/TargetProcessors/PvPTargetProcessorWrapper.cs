using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    /// <summary>
    /// 1. Creates a ITargetProcessor
    /// 2. Cleans up that ITargetProcessor via DisposeManagedState().  The ITargetProcessor
    /// should not be cleaned up by calling code itself!  (Tired offensive bug, where 
    /// GlobalTargetProcessor was being cleaned up on individual offensive deaths.)
    /// </summary>
    public abstract class PvPTargetProcessorWrapper : MonoBehaviour, IManagedDisposable
    {
        private ITargetProcessor _targetProcessor;

        public ITargetProcessor CreateTargetProcessor(IPvPTargetProcessorArgs args)
        {
            _targetProcessor = CreateTargetProcessorInternal(args);
            return _targetProcessor;
        }

        protected abstract ITargetProcessor CreateTargetProcessorInternal(IPvPTargetProcessorArgs args);

        public virtual void DisposeManagedState()
        {
            _targetProcessor?.DisposeManagedState();
            _targetProcessor = null;
        }
    }
}
