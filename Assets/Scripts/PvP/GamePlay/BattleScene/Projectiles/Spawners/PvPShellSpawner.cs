using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPShellSpawner : PvPProjectileSpawner<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        private ITargetFilter _targetFilter;
        private IProjectileSoundPlayerInitialiser soundPlayerInitialiser;
        private SoundType _type;
        private string _name;
        public async Task InitialiseAsync(IPvPProjectileSpawnerArgs args, ISoundKey firingSound, ITargetFilter targetFilter)
        {
            await base.InitialiseAsync(args, firingSound);

            PvPHelper.AssertIsNotNull(targetFilter);
            _targetFilter = targetFilter;
        }

        public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
        {
            Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.MaxVelocityInMPerS);
            PvPProjectileActivationArgs<IPvPProjectileStats> activationArgs
                = new PvPProjectileActivationArgs<IPvPProjectileStats>(
                    transform.position,
                    _projectileStats,
                    shellVelocity,
                    _targetFilter,
                    _parent,
                    _impactSound);
            base.SpawnProjectile(activationArgs);
        }

        protected override void OnProjectileFiredSound(ISoundKey firingSound, int burstSize)
        {
            OnProjectileFiredSoundClientRpc(firingSound.Type, firingSound.Name, burstSize);
        }

        [ClientRpc]
        private void OnProjectileFiredSoundClientRpc(SoundType type, string name, int burstSize)
        {
            if (IsOwner)
            {
                Assert.IsNotNull(soundPlayerInitialiser);
                _type = type;
                _name = name;
                _burstSize = burstSize;
                Invoke("PlayProjectileFiredSound", 0.05f);
            }
        }

        private void Start()
        {
            if (IsClient)
                soundPlayerInitialiser = GetComponent<IProjectileSoundPlayerInitialiser>();
        }

        private async void PlayProjectileFiredSound()
        {
            var soundPlayer
                = await soundPlayerInitialiser?.CreateSoundPlayerAsync(
                    PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayerFactory,
                    new SoundKey(_type, _name),
                    _burstSize,
                    PvPBattleSceneGodClient.Instance.factoryProvider.SettingsManager);
            soundPlayer?.OnProjectileFired();
        }
    }
}
