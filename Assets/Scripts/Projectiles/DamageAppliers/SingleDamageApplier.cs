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
            if (target == null || target.IsDestroyed)
            {
                return;
            }
            try
            {
                target.TakeDamage(_damage, damageSource);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"DAMAGE_FAILED target={target} dmg={_damage:F1} error={ex.Message}");
            }
		}
	}
}
