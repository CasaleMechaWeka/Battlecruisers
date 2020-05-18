using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
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
        private IAudioClipPlayer _audioClipPlayer;
        private IGameObject _audioListener;
        private IAudioClipWrapper _audioClip;
        private ISoundKey _soundKey;

        [SetUp]
        public void SetuUp()
        {
            _soundFetcher = Substitute.For<ISoundFetcher>();
            _audioClipPlayer = Substitute.For<IAudioClipPlayer>();
            _audioListener = Substitute.For<IGameObject>();
            _audioListener.Position.Returns(new Vector3(99, 88, 77));

            _soundPlayer = new SoundPlayer(_soundFetcher, _audioClipPlayer, _audioListener);

            _audioClip = Substitute.For<IAudioClipWrapper>();
            _soundKey = Substitute.For<ISoundKey>();
            _soundFetcher.GetSoundAsync(_soundKey).Returns(Task.FromResult(_audioClip));
        }

        [Test]
        public void PlaySoundAsync_ProvideNoPosition_UsesAudioListenerPosition()
        {
            _soundPlayer.PlaySoundAsync(_soundKey);
            _audioClipPlayer.Received().PlaySound(_audioClip, _audioListener.Position);
        }

        [Test]
        public void PlaySoundAsync_ProvidePosition()
        {
            Vector2 soundPosition = new Vector2(2, 3);
            _soundPlayer.PlaySoundAsync(_soundKey, soundPosition);

            Vector3 zAdjustedPosition = new Vector3(soundPosition.x, soundPosition.y, _audioListener.Position.z);
            _audioClipPlayer.Received().PlaySound(_audioClip, zAdjustedPosition);
        }

        [Test]
        public void PlaySound_ProvideNoPosition_UsesAudioListenerPosition()
        {
            _soundPlayer.PlaySound(_audioClip);
            _audioClipPlayer.Received().PlaySound(_audioClip, _audioListener.Position);
        }

        [Test]
        public void PlaySound_ProvidePosition()
        {
            Vector2 soundPosition = new Vector2(2, 3);
            _soundPlayer.PlaySound(_audioClip, soundPosition);

            Vector3 zAdjustedPosition = new Vector3(soundPosition.x, soundPosition.y, _audioListener.Position.z);
            _audioClipPlayer.Received().PlaySound(_audioClip, zAdjustedPosition);
        }

    }
}
