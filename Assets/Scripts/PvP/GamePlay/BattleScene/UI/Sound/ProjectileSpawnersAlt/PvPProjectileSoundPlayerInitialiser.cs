using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public abstract class PvPProjectileSoundPlayerInitialiser : MonoBehaviour, IPvPProjectileSoundPlayerInitialiser
    {
        public async Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            ISoundPlayerFactory soundPlayerFactory,
            ISoundKey firingSound,
            int burstSize,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(soundPlayerFactory, firingSound, settingsManager);

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);

            IAudioSource audioSourceWrapper
                = new PvPEffectVolumeAudioSource(
                    new PvPAudioSourceBC(audioSource),
                    settingsManager);

            return await CreateSoundPlayerAsync(soundPlayerFactory, firingSound, burstSize, audioSourceWrapper);
        }

        protected abstract Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            ISoundPlayerFactory soundPlayerFactory,
            ISoundKey firingSound,
            int burstSize,
            IAudioSource audioSource);
    }
}
