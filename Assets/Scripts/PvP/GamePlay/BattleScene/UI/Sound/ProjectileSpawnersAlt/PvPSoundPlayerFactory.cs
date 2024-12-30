using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public class PvPSoundPlayerFactory : IPvPSoundPlayerFactory
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IPvPDeferrer _deferrer;

        public IPvPProjectileSpawnerSoundPlayer DummyPlayer { get; }

        public PvPSoundPlayerFactory(ISoundFetcher soundFetcher, IPvPDeferrer deferrer)
        {
            PvPHelper.AssertIsNotNull(soundFetcher, deferrer);

            _soundFetcher = soundFetcher;
            _deferrer = deferrer;

            DummyPlayer = new PvPDummyProjectileSpawnerSoundPlayer();
        }

        public async Task<IPvPProjectileSpawnerSoundPlayer> CreateShortSoundPlayerAsync(ISoundKey firingSound, IPvPAudioSource audioSource)
        {
            IAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(firingSound);
            return new PvPShortSoundPlayer(sound, audioSource);
        }

        public async Task<IPvPProjectileSpawnerSoundPlayer> CreateLongSoundPlayerAsync(ISoundKey firingSound, IPvPAudioSource audioSource, int burstSize, float burstEndDelayInS)
        {
            IAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(firingSound);
            return new PvPLongSoundPlayer(sound, audioSource, _deferrer, burstSize, burstEndDelayInS);
        }
    }
}
