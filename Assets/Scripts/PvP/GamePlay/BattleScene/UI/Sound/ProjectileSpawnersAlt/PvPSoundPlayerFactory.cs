using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public class PvPSoundPlayerFactory : IPvPSoundPlayerFactory
    {
        private readonly IPvPSoundFetcher _soundFetcher;
        private readonly IPvPDeferrer _deferrer;

        public IPvPProjectileSpawnerSoundPlayer DummyPlayer { get; }

        public PvPSoundPlayerFactory(IPvPSoundFetcher soundFetcher, IPvPDeferrer deferrer)
        {
            PvPHelper.AssertIsNotNull(soundFetcher, deferrer);

            _soundFetcher = soundFetcher;
            _deferrer = deferrer;

            DummyPlayer = new PvPDummyProjectileSpawnerSoundPlayer();
        }

        public async Task<IPvPProjectileSpawnerSoundPlayer> CreateShortSoundPlayerAsync(IPvPSoundKey firingSound, IPvPAudioSource audioSource)
        {
            IPvPAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(firingSound);
            return new PvPShortSoundPlayer(sound, audioSource);
        }

        public async Task<IPvPProjectileSpawnerSoundPlayer> CreateLongSoundPlayerAsync(IPvPSoundKey firingSound, IPvPAudioSource audioSource, int burstSize, float burstEndDelayInS)
        {
            IPvPAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(firingSound);
            return new PvPLongSoundPlayer(sound, audioSource, _deferrer, burstSize, burstEndDelayInS);
        }
    }
}
