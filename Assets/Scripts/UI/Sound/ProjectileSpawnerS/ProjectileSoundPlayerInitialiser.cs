using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public abstract class ProjectileSoundPlayerInitialiser : MonoBehaviour, IProjectileSoundPlayerInitialiser
    {
        public IProjectileSpawnerSoundPlayer CreateSoundPlayer(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize)
        {
            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);
            IAudioSource audioSourceWrapper = new AudioSourceBC(audioSource);

            return CreateSoundPlayer(soundPlayerFactory, firingSound, burstSize, audioSourceWrapper);
        }

        protected abstract IProjectileSpawnerSoundPlayer CreateSoundPlayer(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize, IAudioSource audioSource);
    }
}
