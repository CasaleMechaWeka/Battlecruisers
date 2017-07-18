using BattleCruisers.Buildables;

namespace BattleCruisers.Projectiles.DamageAppliers
{
	public interface IDamageApplier
	{
		void DealDamage(ITarget target);
	}
}
