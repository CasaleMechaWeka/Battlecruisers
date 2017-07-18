using BattleCruisers.Buildables;

namespace BattleCruisers.Projectiles.DamageAppliers
{
	public class SingleDamageApplier : IDamageApplier
	{
		private float _damage;

		public SingleDamageApplier(float damage)
		{
			_damage = damage;
		}

		public void ApplyDamage(ITarget target)
		{
			target.TakeDamage(_damage);
		}
	}
}
