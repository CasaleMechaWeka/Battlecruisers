using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;

namespace BattleCruisers.UI.Sound.Wind
{
    // FELIX  Create tests :)
    public class WindManager : IWindManager
    {
        private readonly IAudioSource _audioSource;
        private readonly ICamera _camera;
        private readonly IRange<float> _validOrthographicSizes;

        public WindManager(IAudioSource audioSource, ICamera camera, IRange<float> validOrthographicSizes)
        {
            Helper.AssertIsNotNull(audioSource, camera, validOrthographicSizes);

            _audioSource = audioSource;
            _camera = camera;
            _validOrthographicSizes = validOrthographicSizes;

            _camera.OrthographicSizeChanged += _camera_OrthographicSizeChanged;
        }

        private void _camera_OrthographicSizeChanged(object sender, EventArgs e)
        {
            _audioSource.Volume = FindVolume(_camera.OrthographicSize);
        }

        // FELIX  Extract :)
        private float FindVolume(float cameraOrthographicSize)
        {
            float orthographicSizeProportion = cameraOrthographicSize / _validOrthographicSizes.Max;

            // Don't play wind if less than halfway zoomed out
            if (orthographicSizeProportion < 0.5)
            {
                return 0;
            }

            // OS proportion    Wind volume
            // 0.5              0
            // 0.75             0.5
            // 1                1
            return 2 * orthographicSizeProportion - 1;
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