using UnityEngine;

namespace BattleCruisers.Projectiles.Stats
{
    public class NukeStats : CruisingProjectileStats<NukeController>
	{
		protected override float InitialVelocityMultiplier { get { return 0; } }

        public Vector2 InitialVelocity { get { return new Vector2(0, 0); } }

		public NukeStats(NukeController nukePrefab, float damage, float maxVelocityInMPerS, float cruisingAltitudeInM, float damageRadiusInM)
            : base(nukePrefab, damage, maxVelocityInMPerS, cruisingAltitudeInM, damageRadiusInM)
		{
		}
	}
}
