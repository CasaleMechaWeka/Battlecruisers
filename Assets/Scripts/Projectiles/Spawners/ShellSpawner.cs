using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ShellSpawner : ProjectileSpawner<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        private ITargetFilter _targetFilter;
        private IProjectileSpawnerSoundPlayer _soundPlayer;

        public async Task InitialiseAsync(IProjectileSpawnerArgs args, ITargetFilter targetFilter, ISoundKey firingSound)
        {
            base.Initialise(args);

            Helper.AssertIsNotNull(targetFilter, firingSound);

            _targetFilter = targetFilter;

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
