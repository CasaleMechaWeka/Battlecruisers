namespace BattleCruisers.Targets.TargetProcessors
{
    public class GlobalTargetProcessorWrapper : TargetProcessorWrapper
    {
        protected override ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args)
        {
            return args.TargetsFactory.OffensiveBuildableTargetProcessor;
        }
    }
}
