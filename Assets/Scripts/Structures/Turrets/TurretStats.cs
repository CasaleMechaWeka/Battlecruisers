using System;

public interface ITurretStats
{
	float FireRatePerS { get; }
	float Accuracy { get; }
	int Damage { get; }
	float BulletVelocityInMPerS { get; }
	bool IgnoreGravity { get; }
}

public class TurretStats : ITurretStats
{
	public float FireRatePerS { get; private set; }
	public float Accuracy { get; private set; }
	public int Damage { get; private set; }
	public float BulletVelocityInMPerS { get; private set; }
	public bool IgnoreGravity { get; private set; }

	public TurretStats(float fireRatePerS, float accuracy, int damage, float bulletVelocityInMPerS, bool ignoreGravity)
	{
		if (accuracy <= 0 || accuracy > 1)
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
