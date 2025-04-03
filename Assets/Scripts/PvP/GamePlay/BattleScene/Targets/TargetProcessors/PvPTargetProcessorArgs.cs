using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPTargetProcessorArgs : IPvPTargetProcessorArgs
    {
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public Faction EnemyFaction { get; }
        public IList<TargetType> AttackCapabilities { get; }
        public float MaxRangeInM { get; }
        public float MinRangeInM { get; }
        public ITarget ParentTarget { get; }

        public PvPTargetProcessorArgs(
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            Faction enemyFaction,
            IList<TargetType> attackCapabilities,
            float maxRangeInM,
            float minRangeInM = 0,
            ITarget parentTarget = null)
        {
            PvPHelper.AssertIsNotNull(cruiserSpecificFactories, attackCapabilities);
            Assert.IsTrue(maxRangeInM > minRangeInM,
            "maxRange is not higher than minRange; expected: <" + minRangeInM.ToString() + " actual: " + maxRangeInM.ToString());

            CruiserSpecificFactories = cruiserSpecificFactories;
            EnemyFaction = enemyFaction;
            AttackCapabilities = attackCapabilities;
            MaxRangeInM = maxRangeInM;
            MinRangeInM = minRangeInM;
            ParentTarget = parentTarget;
        }
    }
}
