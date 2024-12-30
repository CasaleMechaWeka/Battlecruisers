using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    public class PvPSoundPlayer : IPvPSoundPlayer
    {
        private readonly IPvPSoundFetcher _soundFetcher;
        private readonly IPvPPool<IPvPAudioSourcePoolable, PvPAudioSourceActivationArgs> _audioSourcePool;

        public PvPSoundPlayer(IPvPSoundFetcher soundFetcher, IPvPPool<IPvPAudioSourcePoolable, PvPAudioSourceActivationArgs> audioSourcePool)
        {
            PvPHelper.AssertIsNotNull(soundFetcher, audioSourcePool);

            _soundFetcher = soundFetcher;
            _audioSourcePool = audioSourcePool;
            _audioSourcePool.SetMaxLimit(21);
        }

        public async Task PlaySoundAsync(ISoundKey soundKey, Vector2 position)
        {
            Assert.IsNotNull(soundKey);
            IAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(soundKey);
            PlaySound(sound, position);
        }

        public void PlaySound(IAudioClipWrapper sound, Vector2 position)
        {
            Assert.IsNotNull(sound);
            PvPAudioSourceActivationArgs activationArgs = new PvPAudioSourceActivationArgs(sound, position);
            _audioSourcePool.GetItem(activationArgs);
        }
    }
}