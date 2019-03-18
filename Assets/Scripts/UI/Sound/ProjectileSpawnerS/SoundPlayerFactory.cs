using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public class SoundPlayerFactory : ISoundPlayerFactory
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IDeferrer _deferrer;

        public SoundPlayerFactory(ISoundFetcher soundFetcher, IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(soundFetcher, deferrer);

            _soundFetcher = soundFetcher;
            _deferrer = deferrer;
        }

        public IProjectileSpawnerSoundPlayer CreateShortSoundPlayer(ISoundKey firingSound, IAudioSource audioSource)
        {
            IAudioClipWrapper sound = _soundFetcher.GetSound(firingSound);
            return new ShortSoundPlayer(sound, audioSource);
        }

        public IProjectileSpawnerSoundPlayer CreateLongSoundPlayer(ISoundKey firingSound, IAudioSource audioSource, int burstSize, float burstEndDelayInS)
        {
            IAudioClipWrapper sound = _soundFetcher.GetSound(firingSound);
            return new LongSoundPlayer(sound, audioSource, _deferrer, burstSize, burstEndDelayInS);
        }
    }
}
