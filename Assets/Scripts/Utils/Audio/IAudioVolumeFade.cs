using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.Utils.Audio
{
    public interface IAudioVolumeFade
    {
        void FadeToVolume(IAudioSource audioSource, float targetVolume, float durationInS);
    }
}