using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils
{
	public class ValueToStarsConverter
	{
		// FELIX  Read from file?
		private readonly int[] HEALTH_STAR_THRESHOLDS = { 0, 50, 150, 300, 1000 };
		private readonly int[] DAMAGE_STAR_THRESHOLDS = { 0, 10, 30, 100, 300 };

		private const int MIN_NUM_OF_STARS = 0;
		private const int MAX_NUM_OF_STARS = 5;

		public int HealthValueToStars(float health)
		{
			return ValueToStars((int)health, HEALTH_STAR_THRESHOLDS);
		}

		public int DamageValueToStars(float damagePerS)
		{
			return ValueToStars((int)damagePerS, DAMAGE_STAR_THRESHOLDS);
		}

		private int ValueToStars(int value, int[] starThresholds)
		{
			for (int i = 0; i < starThresholds.Length; ++i)
			{
				if (value <= starThresholds[i])
				{
					return i;
				}
			}
			return MAX_NUM_OF_STARS;
		}
	}
}
