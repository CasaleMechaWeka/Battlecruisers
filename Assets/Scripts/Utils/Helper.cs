using BattleCruisers.Buildables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils
{
	public static class Helper
	{
		public static Faction GetOppositeFaction(Faction faction)
		{
			return faction == Faction.Blues ? Faction.Reds : Faction.Blues;
		}
	}
}
