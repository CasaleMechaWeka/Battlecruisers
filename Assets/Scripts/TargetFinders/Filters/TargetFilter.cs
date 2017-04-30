using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
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
		private readonly IList<TargetType> _targetTypes;

		public TargetFilter(Faction faction, IList<TargetType> targetTypes)
		{
			_factionToDetect = faction;
			_targetTypes = targetTypes;
		}

		public TargetFilter(Faction faction, params TargetType[] targetTypes)
		{
			_factionToDetect = faction;
			_targetTypes = new List<TargetType>(targetTypes);
		}

		public virtual bool IsMatch(IFactionable factionObject)
		{
			return factionObject.Faction == _factionToDetect
				&& _targetTypes.Contains(factionObject.TargetType);
		}
	}
}
