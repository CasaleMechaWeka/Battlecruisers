public interface IDamagable
{
	void TakeDamage(float damageAmount);
	// FELIX  On fully damaged?
}

public interface IRepairable
{
	void Repair(float repairAmount);
	// FELIX  On fully repaired?
}