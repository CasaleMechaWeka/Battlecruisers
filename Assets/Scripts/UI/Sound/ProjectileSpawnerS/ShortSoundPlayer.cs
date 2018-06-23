using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    // FELIX  Test

    /// <summary>
    /// Plays the projectile firing sound every time a projectile is fired.  For short sounds.
    /// </summary>
    public class ShortSoundPlayer : ProjectileSpawnerSoundPlayer
    {
        public ShortSoundPlayer(IAudioClipWrapper audioClip, IAudioSourceWrapper audioSource)
            : base(audioClip, audioSource)
        {
            Helper.AssertIsNotNull(audioClip, audioSource);
        }

        public override void OnProjectileFired()
        {
            _audioSource.Play();
        }
    }
}
