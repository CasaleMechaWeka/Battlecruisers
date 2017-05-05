using BattleCruisers.Buildables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
	/// <summary>
	/// Prioritise targets by target value.
	/// </summary>
	public class BaseTargetRanker : ITargetRanker
	{
		private const int TARGET_VALUE_MULTIPLIER = 10;

		public virtual int RankTarget(ITarget target)
		{
			return (int)target.TargetValue * TARGET_VALUE_MULTIPLIER;
		}
	}
}
