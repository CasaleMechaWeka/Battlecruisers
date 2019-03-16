using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class BombSpawner : BaseShellSpawner
	{
        public ProjectileController bombPrefab;
        protected override ProjectileController ProjectilePrefab => bombPrefab;

		public void SpawnShell(float currentXVelocityInMPers)
		{
            ProjectileController shell = Instantiate(bombPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
            shell.Initialise(_projectileStats, shellVelocity, _targetFilter, _factoryProvider, _parent);

            base.ShowTrackerIfNeeded(shell);
		}
	}
}
