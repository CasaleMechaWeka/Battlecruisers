using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public class SoundPlayerFactory : ISoundPlayerFactory
    {
        private readonly IDeferrer _deferrer;

        public IProjectileSpawnerSoundPlayer DummyPlayer { get; }

        public SoundPlayerFactory(IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(deferrer);

            _deferrer = deferrer;

            DummyPlayer = new DummyProjectileSpawnerSoundPlayer();
        }

        public async Task<IProjectileSpawnerSoundPlayer> CreateShortSoundPlayerAsync(SoundKey firingSound, IAudioSource audioSource)
        {
            AudioClipWrapper sound = await SoundFetcher.GetSoundAsync(firingSound);
            return new ShortSoundPlayer(sound, audioSource);
        }

        public async Task<IProjectileSpawnerSoundPlayer> CreateLongSoundPlayerAsync(SoundKey firingSound, IAudioSource audioSource, int burstSize, float burstEndDelayInS)
        {
            AudioClipWrapper sound = await SoundFetcher.GetSoundAsync(firingSound);
            return new LongSoundPlayer(sound, audioSource, _deferrer, burstSize, burstEndDelayInS);
        }
    }
}
