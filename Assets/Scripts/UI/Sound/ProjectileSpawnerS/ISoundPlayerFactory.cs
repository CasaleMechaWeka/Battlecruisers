using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public interface ISoundPlayerFactory
    {
        IProjectileSpawnerSoundPlayer CreateShortSoundPlayer(ISoundKey firingSound, IAudioSourceWrapper audioSource);
        IProjectileSpawnerSoundPlayer CreateLongSoundPlayer(ISoundKey firingSound, IAudioSourceWrapper audioSource, int burstSize, float burstEndDelayInS);
    }
}
