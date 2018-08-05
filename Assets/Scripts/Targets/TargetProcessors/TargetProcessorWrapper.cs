using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    // FELIX  Rename to Initialiser/Factory?
    public abstract class TargetProcessorWrapper : MonoBehaviour, ITargetProcessorWrapper
    {
        // FELIX  Simplify args, don't need target consumer anymore :)
        public ITargetProcessor Initialise(ITargetProcessorArgs args)
        {
            Assert.IsNotNull(args);
            return CreateTargetProcessor(args);
        }

        // FELIX  Merge with Initailise() :P
        protected abstract ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args);
    }
}
