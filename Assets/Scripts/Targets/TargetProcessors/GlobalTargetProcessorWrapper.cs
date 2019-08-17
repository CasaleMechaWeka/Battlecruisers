namespace BattleCruisers.Targets.TargetProcessors
{
    public class GlobalTargetProcessorWrapper : TargetProcessorWrapper
    {
        public override ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args)
        {
            return args.CruiserSpecificFactories.Targets.ProcessorFactory.OffensiveBuildableTargetProcessor;
        }
    }
}
