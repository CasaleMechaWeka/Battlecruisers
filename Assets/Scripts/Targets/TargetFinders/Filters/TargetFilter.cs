using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
	public class TargetFilter : ITargetFilter
	{
		private readonly Faction _factionToDetect;
		private readonly TargetType[] _targetTypes;

		public TargetFilter(Faction faction, params TargetType[] targetTypes)
		{
			_factionToDetect = faction;
			_targetTypes = targetTypes;
		}

		public virtual bool IsMatch(ITarget target)
		{
			Logging.Log(Tags.TARGET_FILTER, string.Format("target.Faction: {0}  _factionToDetect: {1}  target.TargetType: {2}", target.Faction, _factionToDetect, target.TargetType));
			return target.Faction == _factionToDetect
				&& _targetTypes.Contains(target.TargetType);
		}
	}
}
