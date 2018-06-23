using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundPlayerFactory
    {
        IProjectileSpawnerSoundPlayer CreateShortSoundPlayer(ISoundKey firingSound, IAudioSourceWrapper audioSource);
        IProjectileSpawnerSoundPlayer CreateShortSoundPlayer(ISoundKey firingSound, IAudioSourceWrapper audioSource, int burstSize, float burstEndDelayInS);
    }
}
