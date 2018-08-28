using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public class ShortSoundPlayerInitialiser : ProjectileSoundPlayerInitialiser
    {
        protected override IProjectileSpawnerSoundPlayer CreateSoundPlayer(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize, IAudioSource audioSource)
        {
            return soundPlayerFactory.CreateShortSoundPlayer(firingSound, audioSource);
        }
    }
}
