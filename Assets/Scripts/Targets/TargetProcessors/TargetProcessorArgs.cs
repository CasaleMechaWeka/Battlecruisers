using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class TargetProcessorArgs : ITargetProcessorArgs
    {
        public ITargetsFactory TargetsFactory { get; private set; }
        public ITargetConsumer TargetConsumer { get; private set; }
        public Faction EnemyFaction { get; private set; }
        public IList<TargetType> AttackCapabilities { get; private set; }
        public float MaxRangeInM { get; private set; }
        public float MinRangeInM { get; private set; }

        public TargetProcessorArgs(
        	ITargetsFactory targetsFactory,
            ITargetConsumer targetConsumer,
            Faction enemyFaction,
            IList<TargetType> attackCapabilities,
            float maxRangeInM,
            float minRangeInM = 0)
        {
            Helper.AssertIsNotNull(targetsFactory, targetConsumer, attackCapabilities);

            TargetsFactory = targetsFactory;
            TargetConsumer = targetConsumer;
            EnemyFaction = enemyFaction;
            AttackCapabilities = attackCapabilities;
            MaxRangeInM = maxRangeInM;
            MinRangeInM = minRangeInM;
        }
    }
}
