using BattleCruisers.Buildables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
	/// <summary>
	/// Prioritises targets by:
	/// 1. Anti air
	/// 2. Anti cruiser
	/// 3. By target value
	/// </summary>
	public class BomberTargetRanker : BaseTargetRanker
	{
		private const int ANTI_AIR_BONUS = 60;
		private const int ANTI_CRUISER_BONUS = 30;

		public override int RankTarget(ITarget target)
		{
			int rank = base.RankTarget(target);

			if (target.AttackCapabilities.Contains(TargetType.Aircraft))
			{
				rank += ANTI_AIR_BONUS;
			}

			if (target.AttackCapabilities.Contains(TargetType.Cruiser))
			{
				rank += ANTI_CRUISER_BONUS;
			}

			return rank;
		}
	}
}
