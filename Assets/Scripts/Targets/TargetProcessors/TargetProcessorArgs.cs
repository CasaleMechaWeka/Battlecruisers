using BattleCruisers.Buildables;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class TargetProcessorArgs : ITargetProcessorArgs
    {
        public ITargetFactoriesProvider TargetFactories { get; private set; }
        public ITargetsFactory TargetsFactory { get; private set; }
        public Faction EnemyFaction { get; private set; }
        public IList<TargetType> AttackCapabilities { get; private set; }
        public float MaxRangeInM { get; private set; }
        public float MinRangeInM { get; private set; }
        public ITarget ParentTarget { get; private set; }

        public TargetProcessorArgs(
            ITargetFactoriesProvider targetFactories,
            ITargetsFactory targetsFactory,
            Faction enemyFaction,
            IList<TargetType> attackCapabilities,
            float maxRangeInM,
            float minRangeInM = 0,
            ITarget parentTarget = null)
        {
            Helper.AssertIsNotNull(targetFactories, targetsFactory, attackCapabilities);
            Assert.IsTrue(maxRangeInM > minRangeInM);

            TargetFactories = targetFactories;
            TargetsFactory = targetsFactory;
            EnemyFaction = enemyFaction;
            AttackCapabilities = attackCapabilities;
            MaxRangeInM = maxRangeInM;
            MinRangeInM = minRangeInM;
            ParentTarget = parentTarget;
        }
    }
}
