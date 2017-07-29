using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using UnityEngine;

namespace BattleCruisers.Utils
{
    public static class Helper
	{
		public static Faction GetOppositeFaction(Faction faction)
		{
			return faction == Faction.Blues ? Faction.Reds : Faction.Blues;
		}

		public static List<IPatrolPoint> ConvertVectorsToPatrolPoints(IList<Vector2> positions)
		{
			return positions
				.Select(position => new PatrolPoint(position) as IPatrolPoint)
				.ToList();
		}
	}
}
