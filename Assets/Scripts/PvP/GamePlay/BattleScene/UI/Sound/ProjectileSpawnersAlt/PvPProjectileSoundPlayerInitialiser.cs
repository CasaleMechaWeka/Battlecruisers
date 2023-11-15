using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public abstract class PvPProjectileSoundPlayerInitialiser : MonoBehaviour, IPvPProjectileSoundPlayerInitialiser
    {
        public async Task<IPvPProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            IPvPSoundPlayerFactory soundPlayerFactory,
            IPvPSoundKey firingSound,
            int burstSize,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(soundPlayerFactory, firingSound, settingsManager);

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);

            IPvPAudioSource audioSourceWrapper
                = new PvPEffectVolumeAudioSource(
                    new PvPAudioSourceBC(audioSource),
                    settingsManager);

            return await CreateSoundPlayerAsync(soundPlayerFactory, firingSound, burstSize, audioSourceWrapper);
        }

        protected abstract Task<IPvPProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            IPvPSoundPlayerFactory soundPlayerFactory,
            IPvPSoundKey firingSound,
            int burstSize,
            IPvPAudioSource audioSource);
    }
}
