using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
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
        private Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> _audioSourcePool;
        private IPoolable<AudioSourceActivationArgs> _audioSource;
        private AudioClipWrapper _sound;
        private ISoundKey _soundKey;
        private Vector2 _soundPosition;
        private AudioSourceActivationArgs _activationArgs;

        [SetUp]
        public void SetuUp()
        {
            _audioSourcePool = Substitute.For<Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>>();

            _sound = Substitute.For<AudioClipWrapper>();
            _soundKey = Substitute.For<ISoundKey>();
            SoundFetcher.GetSoundAsync(_soundKey).Returns(Task.FromResult(_sound));

            _soundPosition = new Vector2(2, 3);
            _activationArgs = new AudioSourceActivationArgs(_sound, _soundPosition);
            _audioSource = Substitute.For<IPoolable<AudioSourceActivationArgs>>();
            _audioSourcePool.GetItem(_activationArgs).Returns(_audioSource);
        }

        [Test]
        public void PlaySoundAsync_ProvidePosition()
        {
            _ = SoundPlayer.PlaySoundAsync(_soundKey, _soundPosition);
            _audioSourcePool.GetItem(_activationArgs).Returns(_audioSource);
        }

        [Test]
        public void PlaySound_ProvidePosition()
        {
            SoundPlayer.PlaySound(_sound.AudioClip, _soundPosition);
            _audioSourcePool.GetItem(_activationArgs).Returns(_audioSource);
        }
    }
}
