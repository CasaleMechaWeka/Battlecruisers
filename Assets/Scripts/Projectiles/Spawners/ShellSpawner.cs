using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ShellSpawner : BaseShellSpawner
	{
        private ISoundKey _spawnSoundKey;

        public SoundKey temp;

        public ProjectileController shellPrefab;
		protected override ProjectileController ProjectilePrefab { get { return shellPrefab; } }

        public void Initialise(IProjectileSpawnerArgs args, ITargetFilter targetFilter, ISoundKey spawnSoundKey)
        {
            base.Initialise(args, targetFilter);

            Assert.IsNotNull(spawnSoundKey);
            _spawnSoundKey = spawnSoundKey;
        }

        public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
            ProjectileController shell = Instantiate(shellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.MaxVelocityInMPerS);
            shell.Initialise(_projectileStats, shellVelocity, _targetFilter, _factoryProvider, _parent);
		}
	}
}
