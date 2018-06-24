using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public class LongSoundPlayerInitialiser : ProjectileSoundPlayerInitialiser
    {
        public float burstEndDelayInS;

        protected override IProjectileSpawnerSoundPlayer CreateSoundPlayer(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize, IAudioSourceWrapper audioSource)
        {
            return soundPlayerFactory.CreateLongSoundPlayer(firingSound, audioSource, burstSize, burstEndDelayInS);
        }
    }
}
