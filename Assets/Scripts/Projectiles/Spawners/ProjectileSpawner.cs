using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class ProjectileSpawner<TProjectile, TProjectileArgs> : MonoBehaviour
        where TProjectile : ProjectileControllerBase
    {
        private IProjectileSpawnerSoundPlayer _soundPlayer;
        public ProjectileType projectileType;
        protected ITarget _parent;
        protected ProjectileStats _projectileStats;
        protected CruiserSpecificFactories _cruiserSpecificFactories;
        protected ICruiser _enemyCruiser;

        protected AudioClipWrapper _impactSound;
        public AudioClip impactSound;

        public List<TargetType> AttackCapabilities { get; set; }

        public async Task InitialiseAsync(IProjectileSpawnerArgs args, ISoundKey firingSound)
        {
            Helper.AssertIsNotNull(impactSound, args);

            _impactSound = new AudioClipWrapper(impactSound);
            _parent = args.Parent;
            _projectileStats = args.ProjectileStats;
            _cruiserSpecificFactories = args.CruiserSpecificFactories;
            _enemyCruiser = args.EnempCruiser;

            IProjectileSoundPlayerInitialiser soundPlayerInitialiser = GetComponent<IProjectileSoundPlayerInitialiser>();
            Assert.IsNotNull(soundPlayerInitialiser);
            _soundPlayer
                = await soundPlayerInitialiser.CreateSoundPlayerAsync(
                    FactoryProvider.Sound.SoundPlayerFactory,
                    firingSound,
                    args.BurstSize,
                    FactoryProvider.SettingsManager);
        }

        protected Vector2 FindProjectileVelocity(float angleInDegrees, bool isSourceMirrored, float velocityInMPerS)
        {
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

            int xDirectionMultiplier = isSourceMirrored ? -1 : 1;

            float velocityX = velocityInMPerS * Mathf.Cos(angleInRadians) * xDirectionMultiplier;
            float velocityY = velocityInMPerS * Mathf.Sin(angleInRadians);

            Logging.Log(Tags.PROJECTILE_SPAWNER, $"angleInDegrees: {angleInDegrees}  isSourceMirrored: {isSourceMirrored}  =>  velocityX: {velocityX}  velocityY: {velocityY}");

            return new Vector2(velocityX, velocityY);
        }

        protected TProjectile SpawnProjectile(ProjectileActivationArgs projectileActivationArgs)
        {
            Assert.IsNotNull(projectileActivationArgs);

            TProjectile projectile = PrefabFactory.GetProjectile<TProjectile>(projectileType, projectileActivationArgs);

            if (_soundPlayer != null)
            {
                _soundPlayer.OnProjectileFired();
            }
            else
            {
                Debug.Log("Warning, soundplayer was null when spawn projectile was called");
            }

            return projectile;
        }
    }
}
