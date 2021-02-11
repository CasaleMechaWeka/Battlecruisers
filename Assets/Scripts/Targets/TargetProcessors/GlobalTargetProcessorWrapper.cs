namespace BattleCruisers.Targets.TargetProcessors
{
    public class GlobalTargetProcessorWrapper : TargetProcessorWrapper
    {
        public override ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args)
        {
            return args.CruiserSpecificFactories.Targets.ProcessorFactory.OffensiveBuildableTargetProcessor;
        }

        public override void DisposeManagedState()
        {
            // Want no clean up, as this is a global target processor shared between many buildables
            // (Remember the tired offensives bug where they  would stop firing? :D )
        }
    }
}
