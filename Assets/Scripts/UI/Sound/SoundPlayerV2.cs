using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound
{
    // FELIX  Renome (remove V2), once legacy sound player is removed :)
    public class SoundPlayerV2 : ISoundPlayer
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IPool<IAudioSourcePoolable, AudioSourceActivationArgs> _audioSourcePool;

        public SoundPlayerV2(ISoundFetcher soundFetcher, IPool<IAudioSourcePoolable, AudioSourceActivationArgs> audioSourcePool)
        {
            Helper.AssertIsNotNull(soundFetcher, audioSourcePool);

            _soundFetcher = soundFetcher;
            _audioSourcePool = audioSourcePool;
        }

        public async Task PlaySoundAsync(ISoundKey soundKey, Vector2 position)
        {
            Assert.IsNotNull(soundKey);

            IAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(soundKey);
            PlaySound(sound, position);
        }

        public void PlaySound(IAudioClipWrapper sound, Vector2 position)
        {
            AudioSourceActivationArgs activationArgs = new AudioSourceActivationArgs(sound, position);
            _audioSourcePool.GetItem(activationArgs);
        }
    }
}