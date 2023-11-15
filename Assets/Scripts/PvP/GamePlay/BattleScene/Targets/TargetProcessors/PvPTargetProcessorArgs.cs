using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPTargetProcessorArgs : IPvPTargetProcessorArgs
    {
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public IPvPTargetFactoriesProvider TargetFactories { get; }
        public PvPFaction EnemyFaction { get; }
        public IList<PvPTargetType> AttackCapabilities { get; }
        public float MaxRangeInM { get; }
        public float MinRangeInM { get; }
        public IPvPTarget ParentTarget { get; }

        public PvPTargetProcessorArgs(
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            IPvPTargetFactoriesProvider targetFactories,
            PvPFaction enemyFaction,
            IList<PvPTargetType> attackCapabilities,
            float maxRangeInM,
            float minRangeInM = 0,
            IPvPTarget parentTarget = null)
        {
            PvPHelper.AssertIsNotNull(cruiserSpecificFactories, targetFactories, attackCapabilities);
            Assert.IsTrue(maxRangeInM > minRangeInM);

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
