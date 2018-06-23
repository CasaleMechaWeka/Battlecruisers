using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.UI.Sound
{
    public abstract class ProjectileSpawnerSoundPlayer : IProjectileSpawnerSoundPlayer
    {
        protected readonly IAudioSourceWrapper _audioSource;

        protected ProjectileSpawnerSoundPlayer(IAudioClipWrapper audioClip, IAudioSourceWrapper audioSource)
        {
            Helper.AssertIsNotNull(audioClip, audioSource);

            _audioSource = audioSource;
            _audioSource.AudioClip = audioClip;
        }

        public abstract void OnProjectileFired();
    }
}