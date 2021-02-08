using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public abstract class ProjectileSoundPlayerInitialiser : MonoBehaviour, IProjectileSoundPlayerInitialiser
    {
        public async Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            ISoundPlayerFactory soundPlayerFactory, 
            ISoundKey firingSound, 
            int burstSize,
            ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(soundPlayerFactory, firingSound, settingsManager);

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);
            IAudioSource audioSourceWrapper 
                = new VolumeAwareAudioSource(
                    new AudioSourceBC(audioSource),
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
