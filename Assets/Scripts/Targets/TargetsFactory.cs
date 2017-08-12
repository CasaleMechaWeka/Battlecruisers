using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;

namespace BattleCruisers.Targets
{
    public class TargetsFactory : ITargetsFactory
	{
		public ITargetProcessor BomberTargetProcessor { get; private set; }
		public ITargetProcessor OffensiveBuildableTargetProcessor { get; private set; }

		public TargetsFactory(ICruiser enemyCruiser)
		{
			BomberTargetProcessor = new TargetProcessor(new GlobalTargetFinder(enemyCruiser), new BomberTargetRanker());
			OffensiveBuildableTargetProcessor = new TargetProcessor(new GlobalTargetFinder(enemyCruiser), new OffensiveBuildableTargetRanker());
		}

		#region TargetProcessors
		public ITargetProcessor CreateTargetProcessor(ITargetFinder targetFinder, ITargetRanker targetRanker)
		{
			return new TargetProcessor(targetFinder, targetRanker);
		}
		#endregion TargetProcessors

		#region TargetFinders
		public ITargetFinder CreateRangedTargetFinder(ITargetDetector targetDetector, ITargetFilter targetFilter)
		{
			return new RangedTargetFinder(targetDetector, targetFilter);
		}
		#endregion TargetFinders

		#region TargetFilters
		public ITargetFilter CreateDetectableTargetFilter(Faction faction, bool isDetectable, IList<TargetType> targetTypes, bool ignoreDestroyedTargets)
		{
            return new DetectableFilter(faction, isDetectable, targetTypes, ignoreDestroyedTargets);
		}

		public ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes, bool ignoreDestroyedTargets)
		{
            return new FactionAndTargetTypeFilter(faction, targetTypes, ignoreDestroyedTargets);
		}

		public IExactMatchTargetFilter CreateExactMatchTargetFilter()
		{
			return new ExactMatchTargetFilter();
		}

		public IExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch)
		{
			return new ExactMatchTargetFilter() 
			{
				Target = targetToMatch
			};
		}

        public ITargetFilter CreateDummyTargetFilter(bool isMatchResult)
        {
            return new DummyTargetFilter(isMatchResult);
        }

		#endregion TargetFilters

		#region TargetRankers
		public ITargetRanker CreateEqualTargetRanker()
		{
			return new EqualTargetRanker();
		}
		#endregion TargetRankers
	}
}
