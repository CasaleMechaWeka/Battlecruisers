using System.Collections.Generic;
using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class GlobalTargetProcessorWrapper : TargetProcessorWrapper
    {
        protected override ITargetProcessor CreateTargetProcessor(
            ITargetsFactory targetsFactory,
            Faction enemyFaction,
			IList<TargetType> attackCapabilities,
            float detectionRangeInM,
            float minRangeInM)
        {
            return targetsFactory.OffensiveBuildableTargetProcessor;
        }
    }
}
