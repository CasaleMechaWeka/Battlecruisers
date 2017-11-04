using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors
{
	public interface ITargetProcessorWrapper : IDisposable
	{
        void StartProvidingTargets(
            ITargetsFactory targetsFactory, 
            ITargetConsumer targetConsumer,
            Faction enemyFaction, 
            float detectionRangeInM, 
            IList<TargetType> attackCapabilities);
	}
}
