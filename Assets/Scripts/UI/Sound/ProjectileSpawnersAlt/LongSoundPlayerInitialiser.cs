using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public class LongSoundPlayerInitialiser : ProjectileSoundPlayerInitialiser
    {
        public float burstEndDelayInS;

        protected override async Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            SoundPlayerFactory soundPlayerFactory,
            SoundKey firingSound,
            int burstSize,
            IAudioSource audioSource)
        {
            return await soundPlayerFactory.CreateLongSoundPlayerAsync(firingSound, audioSource, burstSize, burstEndDelayInS);
        }
    }
}
