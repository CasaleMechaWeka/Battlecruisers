namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPGlobalTargetProcessorWrapper : PvPTargetProcessorWrapper
    {
        protected override IPvPTargetProcessor CreateTargetProcessorInternal(IPvPTargetProcessorArgs args)
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
