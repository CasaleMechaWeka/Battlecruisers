using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ShellSpawner : BaseShellSpawner
	{
        public ProjectileController shellPrefab;
		protected override ProjectileController ProjectilePrefab { get { return shellPrefab; } }

        public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
            ProjectileController shell = Instantiate(shellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.MaxVelocityInMPerS);
            shell.Initialise(_projectileStats, shellVelocity, _targetFilter);
		}
	}
}
