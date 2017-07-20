using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ShellSpawner : BaseShellSpawner
	{
		public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
            ProjectileController shell = Instantiate(_shellStats.ProjectilePrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _shellStats.MaxVelocityInMPerS);
			shell.Initialise(_shellStats, shellVelocity, _targetFilter);
		}
	}
}
