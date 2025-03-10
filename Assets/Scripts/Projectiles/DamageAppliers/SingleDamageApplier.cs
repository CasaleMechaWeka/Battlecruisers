using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles.DamageAppliers
{
	public class SingleDamageApplier : IDamageApplier
	{
        private readonly float _damage;

        public SingleDamageApplier(float damage)
		{
			_damage = damage;
		}

        public void ApplyDamage(ITarget target, Vector2 collisionPoint, ITarget damageSource)
		{
            target.TakeDamage(_damage, damageSource);
		}
	}
}
