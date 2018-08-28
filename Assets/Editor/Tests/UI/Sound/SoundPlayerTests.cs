using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Sound
{
    public class SoundPlayerTests
    {
        private ISoundPlayer _soundPlayer;
        private ISoundFetcher _soundFetcher;
        private IAudioClipPlayer _audioClipPlayer;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _soundFetcher = Substitute.For<ISoundFetcher>();
            _audioClipPlayer = Substitute.For<IAudioClipPlayer>();

            _soundPlayer = new SoundPlayer(_soundFetcher, _audioClipPlayer);
        }

        [Test]
        public void PlaySound()
        {
            IAudioClipWrapper audioClip = Substitute.For<IAudioClipWrapper>();
            ISoundKey soundKey = Substitute.For<ISoundKey>();
            Vector2 soundPosition = new Vector2(2, 3);

            _soundFetcher.GetSound(soundKey).Returns(audioClip);

            _soundPlayer.PlaySound(soundKey, soundPosition);

            _audioClipPlayer.Received().PlaySound(audioClip, soundPosition);
        }
    }
}
