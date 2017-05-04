using BattleCruisers.Buildables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
	/// <summary>
	/// Prioritises targets:
	/// 1. Anti air
	/// 2. Anti cruiser
	/// 3. By target value
	/// </summary>
	public class BomberTargetRanker : ITargetRanker
	{
		private const int TARGET_VALUE_MULTIPLIER = 10;
		private const int ANTI_AIR_BONUS = 60;
		private const int ANTI_CRUISER_BONUS = 30;
		private const int MAX_RANK = 100;

		public int RankTarget(ITarget target)
		{
			int rank = BaseRank(target.TargetValue);

			if (target.AttackCapabilities.Contains(TargetType.Aircraft))
			{
				rank += ANTI_AIR_BONUS;
			}

			if (target.AttackCapabilities.Contains(TargetType.Cruiser))
			{
				rank += ANTI_CRUISER_BONUS;
			}

			if (rank > MAX_RANK)
			{
				rank = MAX_RANK;
			}

			return rank;
		}

		private int BaseRank(TargetValue targetValue)
		{
			return (int)targetValue * TARGET_VALUE_MULTIPLIER;
		}
	}
}
