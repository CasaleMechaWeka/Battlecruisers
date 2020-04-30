using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Sound
{
    public class SoundPlayerTests
    {
        private ISoundPlayer _soundPlayer;
        private ISoundFetcher _soundFetcher;
        private IAudioClipPlayer _audioClipPlayer;
        private ICamera _camera;
        private IAudioClipWrapper _audioClip;
        private ISoundKey _soundKey;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _soundFetcher = Substitute.For<ISoundFetcher>();
            _audioClipPlayer = Substitute.For<IAudioClipPlayer>();
            _camera = Substitute.For<ICamera>();

            // FELIX  fix tests
            _soundPlayer = new SoundPlayer(_soundFetcher, _audioClipPlayer, null);
            //_soundPlayer = new SoundPlayer(_soundFetcher, _audioClipPlayer, _camera);

            _audioClip = Substitute.For<IAudioClipWrapper>();
            _soundKey = Substitute.For<ISoundKey>();
            _soundFetcher.GetSoundAsync(_soundKey).Returns(Task.FromResult(_audioClip));
        }

        [Test]
        public void PlaySound_ProvideNoPosition_UsesMainCameraPosition()
        {
            _camera.Transform.Position.Returns(new Vector3(99, 88, 77));
            _soundPlayer.PlaySoundAsync(_soundKey);
            _audioClipPlayer.Received().PlaySound(_audioClip, _camera.Transform.Position);
        }

        [Test]
        public void PlaySound_ProvidePosition()
        {
            Vector2 soundPosition = new Vector2(2, 3);
            _soundPlayer.PlaySoundAsync(_soundKey, soundPosition);
            _audioClipPlayer.Received().PlaySound(_audioClip, soundPosition);
        }
    }
}
