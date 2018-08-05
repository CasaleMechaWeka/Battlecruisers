using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors
{
    public abstract class TargetProcessorWrapper : MonoBehaviour
    {
        public abstract ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args);
    }
}
