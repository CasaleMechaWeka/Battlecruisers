using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetProcessorFactory : IPvPTargetProcessorFactory
    {
        public IPvPTargetProcessor BomberTargetProcessor { get; }
        public IPvPTargetProcessor OffensiveBuildableTargetProcessor { get; }
        public IPvPTargetProcessor StaticTargetProcessor { get; }

        public PvPTargetProcessorFactory(IPvPCruiser enemyCruiser, IPvPRankedTargetTracker userChosenTargetTracker)
        {
            PvPHelper.AssertIsNotNull(enemyCruiser, userChosenTargetTracker);

            PvPGlobalTargetFinder globalTargetFinder = new PvPGlobalTargetFinder(enemyCruiser);

            BomberTargetProcessor
                = new PvPTargetProcessor(
                    new PvPCompositeTracker(
                        userChosenTargetTracker,
                        new PvPRankedTargetTracker(
                            globalTargetFinder,
                            new PvPBomberTargetRanker())));

            OffensiveBuildableTargetProcessor
                = new PvPTargetProcessor(
                    new PvPCompositeTracker(
                        userChosenTargetTracker,
                        new PvPRankedTargetTracker(
                            globalTargetFinder,
                            new PvPOffensiveBuildableTargetRanker())));

            globalTargetFinder.EmitCruiserAsGlobalTarget();

            StaticTargetProcessor = new PvPStaticTargetProcessor(enemyCruiser);
        }

        public IPvPTargetProcessor CreateTargetProcessor(IPvPRankedTargetTracker rankedTargetTracker)
        {
            return new PvPTargetProcessor(rankedTargetTracker);
        }
    }
}
