using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
    /// <summary>
    /// Prioritise targets by target value.
    /// </summary>
    public class BaseTargetRanker : ITargetRanker
	{
		protected IDictionary<TargetType, int> _attackCapabilityToBonus;

		private const int TARGET_VALUE_MULTIPLIER = 10;
		private const int DEFAULT_ATTACK_CAPABILITY_BONUS = 0;

		public BaseTargetRanker()
		{
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

			return rank;
		}
	}
}
