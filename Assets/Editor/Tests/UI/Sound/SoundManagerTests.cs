using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.UIWrappers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Sound
{
    public class SoundManagerTests
    {
        private ISoundManager _soundManager;
        private ISoundFetcher _soundFetcher;
        private ISoundPlayer _soundPlayer;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _soundFetcher = Substitute.For<ISoundFetcher>();
            _soundPlayer = Substitute.For<ISoundPlayer>();

            _soundManager = new SoundManager(_soundFetcher, _soundPlayer);
        }

        [Test]
        public void PlaySound()
        {
            IAudioClipWrapper audioClip = Substitute.For<IAudioClipWrapper>();
            Vector2 soundPosition = new Vector2(2, 3);
            string soundName = "legit as sound";

            _soundFetcher.GetSound(soundName).Returns(audioClip);

            _soundManager.PlaySound(soundName, soundPosition);

            _soundPlayer.Received().PlaySound(audioClip, soundPosition);
        }
    }
}
