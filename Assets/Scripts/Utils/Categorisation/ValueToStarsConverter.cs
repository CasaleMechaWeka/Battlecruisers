namespace BattleCruisers.Utils.Categorisation
{
    public class ValueToStarsConverter
	{
		private readonly int[] HEALTH_STAR_THRESHOLDS = { 0, 50, 150, 300, 1000 };
		private readonly int[] DAMAGE_STAR_THRESHOLDS = { 0, 10, 30, 100, 300 };

		private const int MIN_NUM_OF_STARS = 0;
		private const int MAX_NUM_OF_STARS = 5;

		public int HealthValueToStars(float health)
		{
			return ValueToStars((int)health, HEALTH_STAR_THRESHOLDS);
		}

        // FELIX  Tailor to target type (Ie, 5 stars for anti cruiser will obviously be more than 5 stars for anti air).
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
