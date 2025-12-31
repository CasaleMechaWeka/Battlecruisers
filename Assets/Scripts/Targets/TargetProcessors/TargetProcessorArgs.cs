using BattleCruisers.Buildables;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class TargetProcessorArgs
    {
        public CruiserSpecificFactories CruiserSpecificFactories { get; }
        public TargetFactoriesProvider TargetFactories { get; }
        public Faction EnemyFaction { get; }
        public IList<TargetType> AttackCapabilities { get; }
        public float MaxRangeInM { get; }
        public float MinRangeInM { get; }
        public ITarget ParentTarget { get; }

        public TargetProcessorArgs(
            CruiserSpecificFactories cruiserSpecificFactories,
            TargetFactoriesProvider targetFactories,
            Faction enemyFaction,
            IList<TargetType> attackCapabilities,
            float maxRangeInM,
            float minRangeInM = 0,
            ITarget parentTarget = null)
        {
            Helper.AssertIsNotNull(cruiserSpecificFactories, targetFactories, attackCapabilities);
            Assert.IsTrue(maxRangeInM > minRangeInM,
            "maxRange is not higher than minRange; expected: <" + minRangeInM.ToString() + " actual: " + maxRangeInM.ToString());

            CruiserSpecificFactories = cruiserSpecificFactories;
            TargetFactories = targetFactories;
            EnemyFaction = enemyFaction;
            AttackCapabilities = attackCapabilities;
            MaxRangeInM = maxRangeInM;
            MinRangeInM = minRangeInM;
            ParentTarget = parentTarget;
        }
    }
}
