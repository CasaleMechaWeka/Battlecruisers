using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors
{
    public abstract class TargetProcessorWrapper : MonoBehaviour, IManagedDisposable
    {
        public abstract ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args);

        public virtual void DisposeManagedState() { }
    }
}
