using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    // FELIX  Make both implementations have no state, so the same damage applier
    // can be reused by a projectile, even if the projectile stats (damage, range, etc) have changed :)
	public interface IDamageApplier
	{
        void ApplyDamage(ITarget target, Vector2 collisionPoint, ITarget damageSource);
	}
}
