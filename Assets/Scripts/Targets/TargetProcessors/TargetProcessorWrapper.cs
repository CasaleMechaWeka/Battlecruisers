using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors
{
    // FELIX  Rename to Initialiser/Factory?  (Need to update all prefabs :/)
    public abstract class TargetProcessorWrapper : MonoBehaviour
    {
        public abstract ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args);
    }
}
