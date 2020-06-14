using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils
{
    public static class Helper
	{
		private const float Y_DEGREES_MIRRORED = 180;
		private const float Y_DEGREES_NOT_MIRRORED = 0;

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

        [Conditional("ENABLE_LOGS")]
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

        public static Quaternion MirrorAccrossYAxis(Quaternion rotation)
        {
            float yDegrees;

            if (rotation.eulerAngles.y == Y_DEGREES_MIRRORED)
            {
                yDegrees = Y_DEGREES_NOT_MIRRORED;
            }
            else if (rotation.eulerAngles.y == Y_DEGREES_NOT_MIRRORED)
            {
                yDegrees = Y_DEGREES_MIRRORED;
            }
            else
            {
                throw new ArgumentException("Expect y angle to be " + Y_DEGREES_MIRRORED + " or " + Y_DEGREES_NOT_MIRRORED + " not " + rotation.eulerAngles.y);
            }

            Vector3 mirroredEulerAngles = new Vector3(rotation.eulerAngles.x, yDegrees, rotation.eulerAngles.z);
            rotation.eulerAngles = mirroredEulerAngles;

            return rotation;
        }

        public static bool IsFacingTarget(Vector2 target, Vector2 source, bool isSourceMirrored)
        {
            return
                isSourceMirrored && target.x < source.x
                || !isSourceMirrored && target.x > source.x;
        }
	}
}
