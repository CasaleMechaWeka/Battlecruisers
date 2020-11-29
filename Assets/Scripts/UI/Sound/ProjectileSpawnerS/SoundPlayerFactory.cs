using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public class SoundPlayerFactory : ISoundPlayerFactory
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IDeferrer _deferrer;

        public IProjectileSpawnerSoundPlayer DummyPlayer { get; }

        public SoundPlayerFactory(ISoundFetcher soundFetcher, IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(soundFetcher, deferrer);

            _soundFetcher = soundFetcher;
            _deferrer = deferrer;

            DummyPlayer = new DummyProjectileSpawnerSoundPlayer();
        }

        public async Task<IProjectileSpawnerSoundPlayer> CreateShortSoundPlayerAsync(ISoundKey firingSound, IAudioSource audioSource)
        {
            IAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(firingSound);
            return new ShortSoundPlayer(sound, audioSource);
        }

        public async Task<IProjectileSpawnerSoundPlayer> CreateLongSoundPlayerAsync(ISoundKey firingSound, IAudioSource audioSource, int burstSize, float burstEndDelayInS)
        {
            IAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(firingSound);
            return new LongSoundPlayer(sound, audioSource, _deferrer, burstSize, burstEndDelayInS);
        }
    }
}
