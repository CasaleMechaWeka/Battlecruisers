using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public abstract class PvPProjectileSpawner<TPvPProjectile, TPvPProjectileArgs, TPvPStats> : MonoBehaviour
        where TPvPProjectile : PvPProjectileControllerBase<TPvPProjectileArgs, TPvPStats>
        where TPvPProjectileArgs : PvPProjectileActivationArgs<TPvPStats>
        where TPvPStats : IPvPProjectileStats
    {
        private IPvPProjectileSpawnerSoundPlayer _soundPlayer;
        private IPvPPool<TPvPProjectile, TPvPProjectileArgs> _projectilePool;

        protected IPvPTarget _parent;
        protected IPvPProjectileStats _projectileStats;
        protected IPvPFactoryProvider _factoryProvider;
        protected IPvPCruiserSpecificFactories _cruiserSpecificFactories;
        protected IPvPCruiser _enemyCruiser;

        protected IPvPAudioClipWrapper _impactSound;
        public AudioClip impactSound;

        public async Task InitialiseAsync(IPvPProjectileSpawnerArgs args, IPvPSoundKey firingSound)
        {
            PvPHelper.AssertIsNotNull(impactSound, args);

            _impactSound = new PvPAudioClipWrapper(impactSound);
            _parent = args.Parent;
            _projectileStats = args.ProjectileStats;
            _factoryProvider = args.FactoryProvider;
            _cruiserSpecificFactories = args.CruiserSpecificFactories;
            _enemyCruiser = args.EnempCruiser;

            IPvPProjectilePoolChooser<TPvPProjectile, TPvPProjectileArgs, TPvPStats> poolChooser = GetComponent<IPvPProjectilePoolChooser<TPvPProjectile, TPvPProjectileArgs, TPvPStats>>();
            Assert.IsNotNull(poolChooser);
            _projectilePool = poolChooser.ChoosePool(args.FactoryProvider.PoolProviders.ProjectilePoolProvider);

            IPvPProjectileSoundPlayerInitialiser soundPlayerInitialiser = GetComponent<IPvPProjectileSoundPlayerInitialiser>();
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

            // Logging.Log(Tags.PROJECTILE_SPAWNER, $"angleInDegrees: {angleInDegrees}  isSourceMirrored: {isSourceMirrored}  =>  velocityX: {velocityX}  velocityY: {velocityY}");

            return new Vector2(velocityX, velocityY);
        }

        protected void SpawnProjectile(TPvPProjectileArgs projectileActivationArgs)
        {
            Assert.IsNotNull(projectileActivationArgs);

            _projectilePool.GetItem(projectileActivationArgs);
            if (_soundPlayer != null)
            {
                _soundPlayer.OnProjectileFired();
            }
            else
            {
                Debug.Log("Warning, soundplayer was null when spawn projectile was called");
            }
        }
    }
}
