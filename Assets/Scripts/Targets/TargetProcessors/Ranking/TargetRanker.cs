using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
	public interface ITargetRanker
	{
		/// <returns>
		/// Ranks the target according to priority.
		/// 
		/// The highest priority is 100, the lowest is 0.
		/// </returns>
		int RankTarget(ITarget target);
	}
}
