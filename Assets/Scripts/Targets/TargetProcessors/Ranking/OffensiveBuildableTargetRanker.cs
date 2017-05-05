using BattleCruisers.Buildables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
	/// <summary>
	/// Prioritises targets by:
	/// 1. Anti cruiser
	/// 2. By target value
	/// </summary>
	public class OffensiveBuildableTargetRanker : BaseTargetRanker 
	{
		private const int ANTI_CRUISER_BONUS = 30;

		public OffensiveBuildableTargetRanker()
		{
			_attackCapabilityToBonus[TargetType.Cruiser] = ANTI_CRUISER_BONUS;
		}
	}
}
