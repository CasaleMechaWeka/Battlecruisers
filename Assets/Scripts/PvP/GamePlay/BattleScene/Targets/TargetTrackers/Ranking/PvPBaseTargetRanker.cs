using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    /// <summary>
    /// Prioritise targets by target value.
    /// </summary>
    public class PvPBaseTargetRanker : IPvPTargetRanker
    {
        protected IDictionary<PvPTargetType, int> _attackCapabilityToBonus;

        private const int TARGET_VALUE_MULTIPLIER = 10;
        private const int DEFAULT_ATTACK_CAPABILITY_BONUS = 0;
        // Being the user chosen target trumps everything :)
        private const int CHOSEN_TARGET_BONUS = 1000;
        public const int MIN_TARGET_RANK = 0;

        public PvPBaseTargetRanker()
        {
            _attackCapabilityToBonus = new Dictionary<PvPTargetType, int>();

            foreach (PvPTargetType attackCapability in Enum.GetValues(typeof(PvPTargetType)))
            {
                _attackCapabilityToBonus.Add(attackCapability, DEFAULT_ATTACK_CAPABILITY_BONUS);
            }
        }

        public int RankTarget(IPvPTarget target)
        {
            int rank = (int)target.TargetValue * TARGET_VALUE_MULTIPLIER;

            foreach (PvPTargetType attackCapability in target.AttackCapabilities)
            {
                rank += _attackCapabilityToBonus[attackCapability];
            }

            return rank;
        }
    }
}
