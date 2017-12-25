using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles.DamageAppliers
{
	public interface IDamageApplier
	{
        void ApplyDamage(ITarget target, Vector2 collisionPoint, ITarget damageSource);
	}
}
