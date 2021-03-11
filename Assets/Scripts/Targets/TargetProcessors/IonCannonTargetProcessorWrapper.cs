using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class IonCannonTargetProcessorWrapper : MonoBehaviour
    {
        public ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args)
        {
            return args.CruiserSpecificFactories.Targets.ProcessorFactory.IonCannonTargetProcessor;
        }
    }
}