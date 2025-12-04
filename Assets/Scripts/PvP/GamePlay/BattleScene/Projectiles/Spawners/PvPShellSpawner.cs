using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPShellSpawner : PvPProjectileSpawner<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>
    {
        private ITargetFilter _targetFilter;

        private SoundType _type;
        private string _name;
        public async Task InitialiseAsync(IPvPProjectileSpawnerArgs args, SoundKey firingSound, ITargetFilter targetFilter)
        {
            await base.InitialiseAsync(args, firingSound);

            PvPHelper.AssertIsNotNull(targetFilter);
            _targetFilter = targetFilter;
        }

        public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
        {
            Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.MaxVelocityInMPerS);
            ProjectileActivationArgs activationArgs
                = new ProjectileActivationArgs(
                    transform.position,
                    _projectileStats,
                    shellVelocity,
                    _targetFilter,
                    _parent,
                    _impactSound);
            base.SpawnProjectile(activationArgs);
        }

        protected override void OnProjectileFiredSound(SoundKey firingSound, int burstSize)
        {
            OnProjectileFiredSoundClientRpc(firingSound.Type, firingSound.Name, burstSize);
        }

        [ClientRpc]
        private void OnProjectileFiredSoundClientRpc(SoundType type, string name, int burstSize)
        {
            if (IsOwner)
            {
                _type = type;
                _name = name;
                _burstSize = burstSize;
                Invoke("PlayProjectileFiredSound", 0.05f);
            }
        }

        private async void PlayProjectileFiredSound()
        {
            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);

            IAudioSource audioSourceWrapper = new EffectVolumeAudioSource(new AudioSourceBC(audioSource));
            IProjectileSpawnerSoundPlayer soundPlayer
                = await PvPFactoryProvider.Sound.SoundPlayerFactory.CreateShortSoundPlayerAsync(new SoundKey(_type, _name), audioSourceWrapper);
        }
    }
}
