using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class BombSpawner : BaseShellSpawner
	{
		public void SpawnShell(float currentXVelocityInMPers)
		{
			Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
            ProjectileActivationArgs<IProjectileStats> activationArgs
                = new ProjectileActivationArgs<IProjectileStats>(
                    transform.position,
                    _projectileStats,
                    shellVelocity,
                    _targetFilter,
                    _parent,
                    _impactSound);
            _projectilePool.GetItem(activationArgs);
		}
	}
}
