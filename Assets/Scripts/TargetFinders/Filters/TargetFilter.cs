using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.TargetFinders.Filters
{
	public interface ITargetFilter
	{
		bool IsMatch(IFactionable factionObject);
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

		public virtual bool IsMatch(IFactionable factionObject)
		{
			return factionObject.Faction == _factionToDetect
				&& _targetTypes.Contains(factionObject.TargetType);
		}
	}
}
