using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public abstract class ProjectileSoundPlayerInitialiser : MonoBehaviour, IProjectileSoundPlayerInitialiser
    {
        public IProjectileSpawnerSoundPlayer CreateSoundPlayer(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(audioSource);
            IAudioSourceWrapper audioSourceWrapper = new AudioSourceWrapper(audioSource);

            return CreateSoundPlayer(soundPlayerFactory, firingSound, burstSize, audioSourceWrapper);
        }

        protected abstract IProjectileSpawnerSoundPlayer CreateSoundPlayer(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize, IAudioSourceWrapper audioSource);
    }
}
