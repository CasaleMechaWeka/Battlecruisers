namespace BattleCruisers.Targets.TargetProcessors
{
    public class StaticTargetProcessorWrapper : TargetProcessorWrapper
    {
        protected override ITargetProcessor CreateTargetProcessorInternal(ITargetProcessorArgs args)
        {
            return args.CruiserSpecificFactories.Targets.ProcessorFactory.StaticTargetProcessor;
        }

        public override void DisposeManagedState()
        {
            // Want no clean up, as this is shared between all ion cannos a user builds
        }
    }
}