using BattleCruisers.Buildables;

namespace BattleCruisers.Projectiles.DamageAppliers
{
	public interface IDamageApplier
	{
		void ApplyDamage(ITarget target);
	}
}
