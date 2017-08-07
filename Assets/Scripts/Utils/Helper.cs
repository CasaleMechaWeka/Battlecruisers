using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using UnityEngine;
using UnityEngine.Assertions;

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

        public static void AssertIsNotNull(params object[] objs)
        {
            foreach (object obj in objs)
            {
                Assert.IsNotNull(obj);
            }
        }

        public static int Half(int number, bool roundUp)
		{
			int half = number / 2;

			if (roundUp)
			{
				half += number % 2;
			}

			return half;
		}
	}
}
