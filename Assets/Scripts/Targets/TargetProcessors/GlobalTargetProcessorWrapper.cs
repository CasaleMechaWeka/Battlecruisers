using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class GlobalTargetProcessorWrapper : TargetProcessorWrapper
    {
        protected override ITargetProcessor CreateTargetProcessor(
            ITargetsFactory targetsFactory,
            ITargetRanker targetRanker,
            Faction enemyFaction,
			IList<TargetType> attackCapabilities,
            float detectionRangeInM,
            float minRangeInM)
        {
            return targetsFactory.OffensiveBuildableTargetProcessor;
        }
    }
}
