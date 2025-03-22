using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Players
{
    public class SoundPlayer : ISoundPlayer
    {
        private readonly SoundFetcher _soundFetcher;
        private readonly IPool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> _audioSourcePool;

        public SoundPlayer(SoundFetcher soundFetcher, IPool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> audioSourcePool)
        {
            Helper.AssertIsNotNull(soundFetcher, audioSourcePool);

            _soundFetcher = soundFetcher;
            _audioSourcePool = audioSourcePool;
            _audioSourcePool.SetMaxLimit(10);
        }

        public async Task PlaySoundAsync(ISoundKey soundKey, Vector2 position)
        {
            Assert.IsNotNull(soundKey);
            AudioClipWrapper sound = await _soundFetcher.GetSoundAsync(soundKey);
            PlaySound(sound, position);
        }

        public void PlaySound(AudioClipWrapper sound, Vector2 position)
        {
            Assert.IsNotNull(sound);
            AudioSourceActivationArgs activationArgs = new AudioSourceActivationArgs(sound, position);
            _audioSourcePool.GetItem(activationArgs);
        }
    }
}