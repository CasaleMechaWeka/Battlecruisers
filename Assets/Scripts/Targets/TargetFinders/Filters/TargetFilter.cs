using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
	// FELIX  Move to own file
	public interface ITargetFilter
	{
		bool IsMatch(ITarget target);
	}

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
			return target.Faction == _factionToDetect
				&& _targetTypes.Contains(target.TargetType);
		}
	}
}
