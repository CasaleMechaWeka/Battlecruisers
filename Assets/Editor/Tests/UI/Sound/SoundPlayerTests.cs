using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Sound
{
    public class SoundPlayerTests
    {
        private ISoundPlayer _soundPlayer;
        private ISoundFetcher _soundFetcher;
        private IPool<IAudioSourcePoolable, AudioSourceActivationArgs> _audioSourcePool;
        private IAudioSourcePoolable _audioSource;
        private IAudioClipWrapper _sound;
        private ISoundKey _soundKey;
        private Vector2 _soundPosition;
        private AudioSourceActivationArgs _activationArgs;

        [SetUp]
        public void SetuUp()
        {
            _soundFetcher = Substitute.For<ISoundFetcher>();
            _audioSourcePool = Substitute.For<IPool<IAudioSourcePoolable, AudioSourceActivationArgs>>();

            _soundPlayer = new SoundPlayer(_soundFetcher, _audioSourcePool);

            _sound = Substitute.For<IAudioClipWrapper>();
            _soundKey = Substitute.For<ISoundKey>();
            _soundFetcher.GetSoundAsync(_soundKey).Returns(Task.FromResult(_sound));
            
            _soundPosition = new Vector2(2, 3);
            _activationArgs = new AudioSourceActivationArgs(_sound, _soundPosition);
            _audioSource = Substitute.For<IAudioSourcePoolable>();
            _audioSourcePool.GetItem(_activationArgs).Returns(_audioSource);
        }

        [Test]
        public void PlaySoundAsync_ProvidePosition()
        {
            _soundPlayer.PlaySoundAsync(_soundKey, _soundPosition);
            _audioSourcePool.GetItem(_activationArgs).Returns(_audioSource);
        }

        [Test]
        public void PlaySound_ProvidePosition()
        {
            _soundPlayer.PlaySound(_sound, _soundPosition);
            _audioSourcePool.GetItem(_activationArgs).Returns(_audioSource);
        }
    }
}
