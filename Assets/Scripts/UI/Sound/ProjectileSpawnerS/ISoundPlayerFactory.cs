using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public interface ISoundPlayerFactory
    {
        IProjectileSpawnerSoundPlayer CreateShortSoundPlayer(ISoundKey firingSound, IAudioSource audioSource);
        IProjectileSpawnerSoundPlayer CreateLongSoundPlayer(ISoundKey firingSound, IAudioSource audioSource, int burstSize, float burstEndDelayInS);
    }
}
