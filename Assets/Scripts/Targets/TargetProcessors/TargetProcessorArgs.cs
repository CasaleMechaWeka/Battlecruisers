using BattleCruisers.Buildables;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class TargetProcessorArgs : ITargetProcessorArgs
    {
        public ITargetFactoriesProvider TargetFactories { get; }
        public Faction EnemyFaction { get; }
        public IList<TargetType> AttackCapabilities { get; }
        public float MaxRangeInM { get; }
        public float MinRangeInM { get; }
        public ITarget ParentTarget { get; }

        public TargetProcessorArgs(
            ITargetFactoriesProvider targetFactories,
            Faction enemyFaction,
            IList<TargetType> attackCapabilities,
            float maxRangeInM,
            float minRangeInM = 0,
            ITarget parentTarget = null)
        {
            Helper.AssertIsNotNull(targetFactories, attackCapabilities);
            Assert.IsTrue(maxRangeInM > minRangeInM);

            TargetFactories = targetFactories;
            EnemyFaction = enemyFaction;
            AttackCapabilities = attackCapabilities;
            MaxRangeInM = maxRangeInM;
            MinRangeInM = minRangeInM;
            ParentTarget = parentTarget;
        }
    }
}
