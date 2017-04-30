using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders.Filters
{
	public interface IFactionObjectFilterFactory
	{
		IFactionObjectFilter CreateFactionFilter(Faction factionToDetect);
		IFactionObjectFilter CreateUnitFilter(Faction faction, UnitCategory unitCategory);
	}

	public class FactionObjectFilterFactory : IFactionObjectFilterFactory
	{
		public IFactionObjectFilter CreateFactionFilter(Faction factionToDetect)
		{
			return new FactionFilter(factionToDetect);
		}

		public IFactionObjectFilter CreateUnitFilter(Faction faction, UnitCategory unitCategory)
		{
			return new UnitFilter(faction, unitCategory);
		}
	}
}
