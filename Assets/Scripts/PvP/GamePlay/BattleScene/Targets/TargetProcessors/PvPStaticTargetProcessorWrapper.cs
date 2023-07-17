namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPStaticTargetProcessorWrapper : PvPTargetProcessorWrapper
    {
        protected override IPvPTargetProcessor CreateTargetProcessorInternal(IPvPTargetProcessorArgs args)
        {
            return args.CruiserSpecificFactories.Targets.ProcessorFactory.StaticTargetProcessor;
        }

        public override void DisposeManagedState()
        {
            // Want no clean up, as this is shared between all ion cannos a user builds
        }
    }
}