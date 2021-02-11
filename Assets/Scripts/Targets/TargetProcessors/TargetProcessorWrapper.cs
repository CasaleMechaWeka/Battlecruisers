using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors
{
    /// <summary>
    /// 1. Creates a ITargetProcessor
    /// 2. Cleans up that ITargetProcessor via DisposeManagedState().  The ITargetProcessor
    /// should not be cleaned up by calling code itself!  (Tired offensive bug, where 
    /// GlobalTargetProcessor was being cleaned up on individual offensive deaths.)
    /// </summary>
    public abstract class TargetProcessorWrapper : MonoBehaviour, IManagedDisposable
    {
        public abstract ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args);

        public virtual void DisposeManagedState() { }
    }
}
