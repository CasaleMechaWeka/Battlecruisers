using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class ProjectileSpawner<TProjectile, TProjectileArgs, TStats> : MonoBehaviour
        where TProjectile : ProjectileControllerBase<TProjectileArgs, TStats>
        where TProjectileArgs : ProjectileActivationArgs<TStats>
        where TStats : IProjectileStats
	{
        private IProjectileSpawnerSoundPlayer _soundPlayer;
        private IPool<TProjectile, TProjectileArgs> _projectilePool;

        protected ITarget _parent;
        protected IProjectileStats _projectileStats;
		protected IFactoryProvider _factoryProvider;
        protected ICruiserSpecificFactories _cruiserSpecificFactories;
        protected ICruiser _enemyCruiser;

        protected IAudioClipWrapper _impactSound;
        public AudioClip impactSound;

        public async Task InitialiseAsync(IProjectileSpawnerArgs args, ISoundKey firingSound)
        {
            Helper.AssertIsNotNull(impactSound, args);

            _impactSound = new AudioClipWrapper(impactSound);
            _parent = args.Parent;
            _projectileStats = args.ProjectileStats;
            _factoryProvider = args.FactoryProvider;
            _cruiserSpecificFactories = args.CruiserSpecificFactories;
            _enemyCruiser = args.EnempCruiser;

            IProjectilePoolChooser<TProjectile, TProjectileArgs, TStats> poolChooser = GetComponent<IProjectilePoolChooser<TProjectile, TProjectileArgs, TStats>>();
            Assert.IsNotNull(poolChooser);
            _projectilePool = poolChooser.ChoosePool(args.FactoryProvider.PoolProviders.ProjectilePoolProvider);

            IProjectileSoundPlayerInitialiser soundPlayerInitialiser = GetComponent<IProjectileSoundPlayerInitialiser>();
            Assert.IsNotNull(soundPlayerInitialiser);
            _soundPlayer 
                = await soundPlayerInitialiser.CreateSoundPlayerAsync(
                    args.FactoryProvider.Sound.SoundPlayerFactory, 
                    firingSound, 
                    args.BurstSize,
                    args.FactoryProvider.SettingsManager);
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

        protected void SpawnProjectile(TProjectileArgs projectileActivationArgs)
        {
            Assert.IsNotNull(projectileActivationArgs);

            _projectilePool.GetItem(projectileActivationArgs);
            if (_soundPlayer != null)
            {
                _soundPlayer.OnProjectileFired();
            }
            else{
                Debug.Log("Warning, soundplayer was null when spawn projectile was called");
            }
        }
    }
}
