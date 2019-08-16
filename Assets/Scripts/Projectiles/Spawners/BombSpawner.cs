using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class BombSpawner : BaseShellSpawner
	{
        public ProjectileController bombPrefab;
        protected override MonoBehaviour ProjectilePrefab => bombPrefab;

		public void SpawnShell(float currentXVelocityInMPers)
		{
			Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
            ProjectileActivationArgs<IProjectileStats> activationArgs
                = new ProjectileActivationArgs<IProjectileStats>(
                    transform.position,
                    _projectileStats,
                    shellVelocity,
                    _targetFilter,
                    _parent);
            _projectilePool.GetItem(activationArgs);

            // FELIX  Remove :P
            //base.ShowTrackerIfNeeded(shell);
		}
	}
}
