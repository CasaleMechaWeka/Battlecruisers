using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors
{
	public interface ITargetProcessorWrapper : IDisposable
	{
        void Initialise(
            ITargetsFactory targetsFactory, 
            ITargetConsumer targetConsumer,
            Faction enemyFaction, 
			IList<TargetType> attackCapabilities,
            float detectionRangeInM,
            float minRangeInM = 0);

        void StartProvidingTargets();
	}
}
