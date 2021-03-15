using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.Factories
{
    public class TargetProcessorFactory : ITargetProcessorFactory
    {
        public ITargetProcessor BomberTargetProcessor { get; }
        public ITargetProcessor OffensiveBuildableTargetProcessor { get; }
        public ITargetProcessor StaticTargetProcessor { get; }

        public TargetProcessorFactory(ICruiser enemyCruiser, IRankedTargetTracker userChosenTargetTracker)
		{
            Helper.AssertIsNotNull(enemyCruiser, userChosenTargetTracker);

            GlobalTargetFinder globalTargetFinder = new GlobalTargetFinder(enemyCruiser);

            BomberTargetProcessor 
                = new TargetProcessor(
                    new CompositeTracker(
                        userChosenTargetTracker,
                        new RankedTargetTracker(
                            globalTargetFinder, 
                            new BomberTargetRanker())));

            OffensiveBuildableTargetProcessor
                = new TargetProcessor(
                    new CompositeTracker(
                        userChosenTargetTracker,
                        new RankedTargetTracker(
                            globalTargetFinder,
                            new OffensiveBuildableTargetRanker())));

            globalTargetFinder.EmitCruiserAsGlobalTarget();

            StaticTargetProcessor = new StaticTargetProcessor(enemyCruiser);
		}

        public ITargetProcessor CreateTargetProcessor(IRankedTargetTracker rankedTargetTracker)
		{
			return new TargetProcessor(rankedTargetTracker);
		}
    }
}
