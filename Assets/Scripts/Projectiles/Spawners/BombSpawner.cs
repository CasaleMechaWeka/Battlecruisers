using BattleCruisers.Projectiles.ActivationArgs;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class BombSpawner : BaseShellSpawner
	{
        public ProjectileController bombPrefab;
        protected override MonoBehaviour ProjectilePrefab => bombPrefab;

		public void SpawnShell(float currentXVelocityInMPers)
		{
            ProjectileController shell = Instantiate(bombPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
            shell.Initialise(_factoryProvider);
            shell.Activate(
                new ProjectileActivationArgs<Stats.IProjectileStats>(
                    _projectileStats,
                    shellVelocity,
                    _targetFilter,
                    _parent));

            base.ShowTrackerIfNeeded(shell);
		}
	}
}
