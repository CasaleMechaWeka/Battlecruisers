public class TurretStats : ITurretStats
{
	public float FireRatePerS { get; private set; }
	public float Accuracy { get; private set; }
	public int Damage { get; private set; }

	public TurretStats(float fireRatePerS, float accuracy, int damage)
	{
		FireRatePerS = fireRatePerS;
		Accuracy = accuracy;
		Damage = damage;
	}
}
