using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound;
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

        public async Task PlaySoundAsync(IPvPSoundKey soundKey, Vector2 position)
        {
            Assert.IsNotNull(soundKey);
            IPvPAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(soundKey);
            PlaySound(sound, position);
        }

        public void PlaySound(IPvPAudioClipWrapper sound, Vector2 position)
        {
            Assert.IsNotNull(sound);
            PvPAudioSourceActivationArgs activationArgs = new PvPAudioSourceActivationArgs(sound, position);
            _audioSourcePool.GetItem(activationArgs);
        }
    }
}