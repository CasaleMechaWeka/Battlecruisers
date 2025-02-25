using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    public class PvPSoundPlayer : IPvPSoundPlayer
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IPvPPool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> _audioSourcePool;

        public PvPSoundPlayer(ISoundFetcher soundFetcher, IPvPPool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> audioSourcePool)
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
            AudioSourceActivationArgs activationArgs = new AudioSourceActivationArgs(sound, position);
            _audioSourcePool.GetItem(activationArgs);
        }
    }
}