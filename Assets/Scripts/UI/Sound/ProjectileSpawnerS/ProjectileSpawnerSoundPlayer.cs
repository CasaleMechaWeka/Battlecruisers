using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public abstract class ProjectileSpawnerSoundPlayer : IProjectileSpawnerSoundPlayer
    {
        protected readonly IAudioSource _audioSource;

        protected ProjectileSpawnerSoundPlayer(IAudioClipWrapper audioClip, IAudioSource audioSource)
        {
            Helper.AssertIsNotNull(audioClip, audioSource);

            _audioSource = audioSource;
            _audioSource.AudioClip = audioClip;
        }

        public abstract void OnProjectileFired();
    }
}
