using BattleCruisers.Utils;
using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Projectiles.Spawners.Laser
{
    public class LaserSoundPlayer : ILaserSoundPlayer
    {
        private readonly ILaserRenderer _laserRenderer;
        private readonly IAudioSourceWrapper _audioSource;
        private readonly IAudioClipWrapper _laserSound;

        public LaserSoundPlayer(ILaserRenderer laserRenderer, IAudioSourceWrapper audioSource, IAudioClipWrapper laserSound)
        {
            Helper.AssertIsNotNull(laserRenderer, audioSource, laserSound);

            _laserRenderer = laserRenderer;
            _audioSource = audioSource;
            _laserSound = laserSound;

            _laserRenderer.LaserVisibilityChanged += _laserRenderer_LaserVisibilityChanged;

            _audioSource.AudioClip = _laserSound;
        }

        private void _laserRenderer_LaserVisibilityChanged(object sender, LaserVisibilityChangedEventArgs e)
        {
            if (e.IsLaserVisible)
            {
                _audioSource.Play(isSpatial: true, loop: false);
            }
            else
            {
                _audioSource.Stop();
            }
        }

        public void DisposeManagedState()
        {
            _laserRenderer.LaserVisibilityChanged -= _laserRenderer_LaserVisibilityChanged;
        }
    }
}
