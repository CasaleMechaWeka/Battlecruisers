using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound
{
    public class SingleSoundPlayerTests
    {
        private ISingleSoundPlayer _soundPlayer;
        private ISoundFetcher _soundFetcher;
        private IAudioSource _audioSource;
        private ISoundKey _soundKey;
        private IAudioClipWrapper _soundToPlay;

        [SetUp]
        public void TestSetup()
        {
            _soundFetcher = Substitute.For<ISoundFetcher>();
            _audioSource = Substitute.For<IAudioSource>();
            _soundPlayer = new SingleSoundPlayer(_soundFetcher, _audioSource);

            _soundKey = Substitute.For<ISoundKey>();
            _soundToPlay = Substitute.For<IAudioClipWrapper>();

            _soundFetcher.GetSound(_soundKey).Returns(_soundToPlay);
        }

        [Test]
        public void PlaySound()
        {
            _soundPlayer.PlaySound(_soundKey);

            _audioSource.Received().Stop();
            _audioSource.Received().AudioClip = _soundToPlay;
            _audioSource.Received().Play(isSpatial: false);
        }

        [Test]
        public void Stop()
        {
            _soundPlayer.Stop();
            _audioSource.Received().Stop();
        }
    }
}