using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ShellSpawner : BaseShellSpawner
	{
        private ISoundKey _firingSound;
        private ISoundManager _soundManager;

        public ProjectileController shellPrefab;
		protected override ProjectileController ProjectilePrefab { get { return shellPrefab; } }

        public void Initialise(IProjectileSpawnerArgs args, ITargetFilter targetFilter, ISoundKey firingSound)
        {
            base.Initialise(args, targetFilter);

            Assert.IsNotNull(firingSound);
            _firingSound = firingSound;

            _soundManager = args.FactoryProvider.SoundManager;
        }

        public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
            ProjectileController shell = Instantiate(shellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.MaxVelocityInMPerS);
            shell.Initialise(_projectileStats, shellVelocity, _targetFilter, _factoryProvider, _parent);
            _soundManager.PlaySound(_firingSound, transform.position);
		}
	}
}
