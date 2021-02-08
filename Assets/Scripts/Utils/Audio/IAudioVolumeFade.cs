using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.Utils.Audio
{
    public interface IAudioVolumeFade
    {
        void FadeToVolume(IAudioSource audioSource, float targetVolume, float durationInS);

        /// <summary>
        /// For cutting the fade short.  Useful when the user has changed the volume, and we
        /// want to instantly go to their new desired volume.  Ok to call if not during fade.
        /// </summary>
        void Stop();
    }
}