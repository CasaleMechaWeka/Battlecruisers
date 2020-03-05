using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ShellSpawner : BaseShellSpawner
	{
        private IProjectileSpawnerSoundPlayer _soundPlayer;

        public async Task InitialiseAsync(IProjectileSpawnerArgs args, ITargetFilter targetFilter, ISoundKey firingSound)
        {
            base.Initialise(args, targetFilter);

            Assert.IsNotNull(firingSound);

            IProjectileSoundPlayerInitialiser soundPlayerInitialiser = GetComponent<IProjectileSoundPlayerInitialiser>();
            Assert.IsNotNull(soundPlayerInitialiser);
            _soundPlayer = await soundPlayerInitialiser.CreateSoundPlayerAsync(args.FactoryProvider.Sound.SoundPlayerFactory, firingSound, args.BurstSize);
        }

        public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
			Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.MaxVelocityInMPerS);
            ProjectileActivationArgs<IProjectileStats> activationArgs
                = new ProjectileActivationArgs<IProjectileStats>(
                    transform.position,
                    _projectileStats,
                    shellVelocity,
                    _targetFilter,
                    _parent,
                    _impactSound);
            _projectilePool.GetItem(activationArgs);

            _soundPlayer.OnProjectileFired();
		}
	}
}
