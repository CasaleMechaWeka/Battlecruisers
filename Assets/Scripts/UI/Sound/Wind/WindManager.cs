using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;

namespace BattleCruisers.UI.Sound.Wind
{
    public class WindManager : IWindManager
    {
        private readonly IAudioSource _audioSource;
        private readonly ICamera _camera;
        private readonly IVolumeCalculator _volumeCalculator;

        public WindManager(IAudioSource audioSource, ICamera camera, IVolumeCalculator volumeCalculator)
        {
            Helper.AssertIsNotNull(audioSource, camera, volumeCalculator);

            _audioSource = audioSource;
            _camera = camera;
            _volumeCalculator = volumeCalculator;

            _camera.OrthographicSizeChanged += _camera_OrthographicSizeChanged;
        }

        private void _camera_OrthographicSizeChanged(object sender, EventArgs e)
        {
            _audioSource.Volume = _volumeCalculator.FindVolume(_camera.OrthographicSize);
        }

        public void Play()
        {
            _audioSource.Play(isSpatial: false, loop: true);
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}