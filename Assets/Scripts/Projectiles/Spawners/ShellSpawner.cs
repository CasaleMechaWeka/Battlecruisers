using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ShellSpawner : BaseShellSpawner
	{
        private IProjectileSpawnerSoundPlayer _soundPlayer;

        public ProjectileController shellPrefab;
		protected override ProjectileController ProjectilePrefab { get { return shellPrefab; } }

        public void Initialise(IProjectileSpawnerArgs args, ITargetFilter targetFilter, ISoundKey firingSound)
        {
            base.Initialise(args, targetFilter);

            Assert.IsNotNull(firingSound);

            IProjectileSoundPlayerInitialiser soundPlayerInitialiser = GetComponent<IProjectileSoundPlayerInitialiser>();
            Assert.IsNotNull(soundPlayerInitialiser);
            _soundPlayer = soundPlayerInitialiser.CreateSoundPlayer(args.FactoryProvider.Sound.SoundPlayerFactory, firingSound, args.BurstSize);
        }

        public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
            ProjectileController shell = Instantiate(shellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.MaxVelocityInMPerS);
            shell.Initialise(_projectileStats, shellVelocity, _targetFilter, _factoryProvider, _parent);
            _soundPlayer.OnProjectileFired();

            base.ShowTrackerIfNeeded(shell);
		}
	}
}
