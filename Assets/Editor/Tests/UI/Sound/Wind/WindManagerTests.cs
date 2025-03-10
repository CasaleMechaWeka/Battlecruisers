using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound.Wind
{
    public class WindManagerTests
    {
        private IWindManager _manager;
        private IAudioSource _audioSource;
        private ICamera _camera;
        private IVolumeCalculator _volumeCalculator;
        private ISettingsManager _settingsManager;
        private float _volume;

        [SetUp]
        public void TestSetup()
        {
            _audioSource = Substitute.For<IAudioSource>();
            _camera = Substitute.For<ICamera>();
            _volumeCalculator = Substitute.For<IVolumeCalculator>();
            _settingsManager = Substitute.For<ISettingsManager>();

            _volume = 71.2f;
            _camera.OrthographicSize.Returns(17);
            _volumeCalculator.FindVolume(_camera.OrthographicSize).Returns(_volume);

            _manager = new WindManager(_audioSource, _camera, _volumeCalculator, _settingsManager);
        }

        [Test]
        public void InitialState()
        {
            _audioSource.Received().Volume = _volume;
        }

        [Test]
        public void _camera_OrthographicSizeChanged()
        {
            _audioSource.ClearReceivedCalls();

            _camera.OrthographicSizeChanged += Raise.Event();

            _audioSource.Received().Volume = _volume;
        }

        [Test]
        public void _settingsManager_SettingsSaved()
        {
            _audioSource.ClearReceivedCalls();

            _settingsManager.SettingsSaved += Raise.Event();

            _audioSource.Received().Volume = _volume;
        }

        [Test]
        public void Stop()
        {
            _manager.Stop();
            _audioSource.Received().Stop();
        }

        [Test]
        public void Play()
        {
            _manager.Play();
            _audioSource.Received().Play(isSpatial: false, loop: true);
        }
    }
}