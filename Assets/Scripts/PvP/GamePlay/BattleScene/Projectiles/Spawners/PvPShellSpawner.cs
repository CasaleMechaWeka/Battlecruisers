using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPShellSpawner : PvPProjectileSpawner<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        private IPvPTargetFilter _targetFilter;
        private IPvPProjectileSoundPlayerInitialiser soundPlayerInitialiser;
        private PvPSoundType _type;
        private string _name;
        public async Task InitialiseAsync(IPvPProjectileSpawnerArgs args, IPvPSoundKey firingSound, IPvPTargetFilter targetFilter)
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

        protected override void OnProjectileFiredSound(IPvPSoundKey firingSound, int burstSize)
        {
            OnProjectileFiredSoundClientRpc(firingSound.Type, firingSound.Name, burstSize);
        }

        [ClientRpc]
        private void OnProjectileFiredSoundClientRpc(PvPSoundType type, string name, int burstSize)
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
                soundPlayerInitialiser = GetComponent<IPvPProjectileSoundPlayerInitialiser>();
        }

        private async void PlayProjectileFiredSound()
        {
            var soundPlayer
                = await soundPlayerInitialiser?.CreateSoundPlayerAsync(
                    PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayerFactory,
                    new PvPSoundKey(_type, _name),
                    _burstSize,
                    PvPBattleSceneGodClient.Instance.factoryProvider.SettingsManager);
            soundPlayer?.OnProjectileFired();
        }
    }
}
