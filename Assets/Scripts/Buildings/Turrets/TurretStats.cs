using System;

namespace BattleCruisers.Buildings.Turrets
{
	public interface ITurretStats
	{
		float FireRatePerS { get; }
		float Accuracy { get; }
		float Damage { get; }
		float BulletVelocityInMPerS { get; }
		bool IgnoreGravity { get; }
		float DamangePerS { get; }
	}

	public class TurretStats : ITurretStats
	{
		public float FireRatePerS { get; private set; }
		public float Accuracy { get; private set; }
		public float Damage { get; private set; }
		public float BulletVelocityInMPerS { get; private set; }
		public bool IgnoreGravity { get; private set; }
		public float DamangePerS { get { return Damage * FireRatePerS; } }

		public TurretStats(float fireRatePerS, float accuracy, float damage, float bulletVelocityInMPerS, bool ignoreGravity)
		{
			if (fireRatePerS < 0
				|| accuracy <= 0 
				|| accuracy > 1
				|| damage < 0
				|| bulletVelocityInMPerS < 0)
			{
				throw new ArgumentException();
			}

			FireRatePerS = fireRatePerS;
			Accuracy = accuracy;
			Damage = damage;
			BulletVelocityInMPerS = bulletVelocityInMPerS;
			IgnoreGravity = ignoreGravity;
		}
	}
}