using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetProcessorFactory : ITargetProcessorFactory
    {
        public ITargetProcessor BomberTargetProcessor { get; }
        public ITargetProcessor OffensiveBuildableTargetProcessor { get; }
        public ITargetProcessor StaticTargetProcessor { get; }

        public PvPTargetProcessorFactory(IPvPCruiser enemyCruiser, IRankedTargetTracker userChosenTargetTracker)
        {
            PvPHelper.AssertIsNotNull(enemyCruiser, userChosenTargetTracker);

            PvPGlobalTargetFinder globalTargetFinder = new PvPGlobalTargetFinder(enemyCruiser);

            BomberTargetProcessor
                = new PvPTargetProcessor(
                    new PvPCompositeTracker(
                        userChosenTargetTracker,
                        new PvPRankedTargetTracker(
                            globalTargetFinder,
                            new BomberTargetRanker())));

            OffensiveBuildableTargetProcessor
                = new PvPTargetProcessor(
                    new PvPCompositeTracker(
                        userChosenTargetTracker,
                        new PvPRankedTargetTracker(
                            globalTargetFinder,
                            new OffensiveBuildableTargetRanker())));

            globalTargetFinder.EmitCruiserAsGlobalTarget();

            StaticTargetProcessor = new PvPStaticTargetProcessor(enemyCruiser);
        }

        public ITargetProcessor CreateTargetProcessor(IRankedTargetTracker rankedTargetTracker)
        {
            return new PvPTargetProcessor(rankedTargetTracker);
        }
    }
}
