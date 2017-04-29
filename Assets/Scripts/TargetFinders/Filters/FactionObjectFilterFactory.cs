using BattleCruisers.Buildables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders.Filters
{
	public interface IFactionObjectFilterFactory
	{
		IFactionObjectFilter CreateFactionFilter(Faction factionToDetect);
	}

	public class FactionObjectFilterFactory : IFactionObjectFilterFactory
	{
		public IFactionObjectFilter CreateFactionFilter(Faction factionToDetect)
		{
			return new FactionFilter(factionToDetect);
		}
	}
}
