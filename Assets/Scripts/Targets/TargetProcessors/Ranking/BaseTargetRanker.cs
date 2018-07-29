using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProviders;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
    /// <summary>
    /// Prioritise targets by target value.
    /// </summary>
    public class BaseTargetRanker : ITargetRanker
	{
        private readonly ITargetProvider _userChosenTargetProvider;
        protected IDictionary<TargetType, int> _attackCapabilityToBonus;

		private const int TARGET_VALUE_MULTIPLIER = 10;
		private const int DEFAULT_ATTACK_CAPABILITY_BONUS = 0;
        // Being the user chosen target trumps everything :)
		private const int CHOSEN_TARGET_BONUS = 1000;
        public const int MIN_TARGET_RANK = 0;

		public BaseTargetRanker(ITargetProvider userChosenTargetProvider)
		{
            Assert.IsNotNull(userChosenTargetProvider);
            _userChosenTargetProvider = userChosenTargetProvider;

			_attackCapabilityToBonus = new Dictionary<TargetType, int>();

			foreach (TargetType attackCapability in Enum.GetValues(typeof(TargetType)))
			{
				_attackCapabilityToBonus.Add(attackCapability, DEFAULT_ATTACK_CAPABILITY_BONUS);
			}
		}

		public int RankTarget(ITarget target)
		{
			int rank = (int)target.TargetValue * TARGET_VALUE_MULTIPLIER;

			foreach (TargetType attackCapability in target.AttackCapabilities)
			{
				rank += _attackCapabilityToBonus[attackCapability];
			}

            if (ReferenceEquals(_userChosenTargetProvider.Target, target))
            {
                rank += CHOSEN_TARGET_BONUS;
            }

            return rank;
		}
	}
}
