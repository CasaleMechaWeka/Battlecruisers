using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders
{
	public class FactionFilter : IFactionObjectFilter
	{
		private readonly Faction _factionToDetect;

		public FactionFilter(Faction factionToDetect)
		{
			_factionToDetect = factionToDetect;
		}

		public bool IsMatch(IFactionable factionObject)
		{
			return factionObject.Faction == _factionToDetect;
		}
	}
}
