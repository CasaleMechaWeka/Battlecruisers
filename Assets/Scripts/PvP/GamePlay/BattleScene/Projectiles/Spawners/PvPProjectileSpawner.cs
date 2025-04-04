using BattleCruisers.Buildables;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public abstract class PvPProjectileSpawner<TPvPProjectile, TPvPProjectileArgs, TPvPStats> : NetworkBehaviour
        where TPvPProjectile : PvPProjectileControllerBase<TPvPProjectileArgs, TPvPStats>
        where TPvPProjectileArgs : ProjectileActivationArgs<TPvPStats>
        where TPvPStats : IProjectileStats
    {
        private IProjectileSpawnerSoundPlayer _soundPlayer;
        private Pool<TPvPProjectile, TPvPProjectileArgs> _projectilePool;

        protected ITarget _parent;
        protected IProjectileStats _projectileStats;
        protected IPvPCruiserSpecificFactories _cruiserSpecificFactories;
        protected IPvPCruiser _enemyCruiser;

        protected AudioClipWrapper _impactSound;
        public AudioClip impactSound;

        private ISoundKey _firingSound;
        protected int _burstSize;

        public async Task InitialiseAsync(IPvPProjectileSpawnerArgs args, ISoundKey firingSound)
        {

            PvPHelper.AssertIsNotNull(impactSound, args);

            _impactSound = new AudioClipWrapper(impactSound);
            _parent = args.Parent;
            _projectileStats = args.ProjectileStats;
            _cruiserSpecificFactories = args.CruiserSpecificFactories;
            _enemyCruiser = args.EnempCruiser;

            IPvPProjectilePoolChooser<TPvPProjectile, TPvPProjectileArgs, TPvPStats> poolChooser = GetComponent<IPvPProjectilePoolChooser<TPvPProjectile, TPvPProjectileArgs, TPvPStats>>();

            Assert.IsNotNull(poolChooser);

            _projectilePool = poolChooser.ChoosePool(PvPFactoryProvider.PoolProviders.ProjectilePoolProvider);

            IProjectileSoundPlayerInitialiser soundPlayerInitialiser = GetComponent<IProjectileSoundPlayerInitialiser>();
            Assert.IsNotNull(soundPlayerInitialiser);
            _soundPlayer
                = await soundPlayerInitialiser.CreateSoundPlayerAsync(
                    PvPFactoryProvider.Sound.SoundPlayerFactory,
                    firingSound,
                    args.BurstSize,
                    DataProvider.SettingsManager);
            _firingSound = firingSound;
            _burstSize = args.BurstSize;

        }

        protected Vector2 FindProjectileVelocity(float angleInDegrees, bool isSourceMirrored, float velocityInMPerS)
        {
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

            int xDirectionMultiplier = isSourceMirrored ? -1 : 1;

            float velocityX = velocityInMPerS * Mathf.Cos(angleInRadians) * xDirectionMultiplier;
            float velocityY = velocityInMPerS * Mathf.Sin(angleInRadians);

            // Logging.Log(Tags.PROJECTILE_SPAWNER, $"angleInDegrees: {angleInDegrees}  isSourceMirrored: {isSourceMirrored}  =>  velocityX: {velocityX}  velocityY: {velocityY}");

            return new Vector2(velocityX, velocityY);
        }

        protected void SpawnProjectile(TPvPProjectileArgs projectileActivationArgs)
        {
            Assert.IsNotNull(projectileActivationArgs);

            _projectilePool.GetItem(projectileActivationArgs);
            if (_firingSound != null && _soundPlayer != null)
            {
                _soundPlayer.OnProjectileFired();
                OnProjectileFiredSound(_firingSound, _burstSize);
            }
            else
            {
                Debug.LogWarning("Warning, soundplayer was null when spawn projectile was called");
            }
        }
        protected virtual void OnProjectileFiredSound(ISoundKey firingSound, int burstSize) { }
    }
}