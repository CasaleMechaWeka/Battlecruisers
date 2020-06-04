
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Projectiles.Spawners.Beams.Laser
{
    public class LaserSoundPlayer : ILaserSoundPlayer
    {
        private readonly ILaserRenderer _laserRenderer;
        private readonly IAudioSource _audioSource;

        public LaserSoundPlayer(ILaserRenderer laserRenderer, IAudioSource audioSource)
        {
            Helper.AssertIsNotNull(laserRenderer, audioSource);

            _laserRenderer = laserRenderer;
            _audioSource = audioSource;

            _laserRenderer.LaserVisibilityChanged += _laserRenderer_LaserVisibilityChanged;
        }

        private void _laserRenderer_LaserVisibilityChanged(object sender, LaserVisibilityChangedEventArgs e)
        {
            if (e.IsLaserVisible)
            {
                _audioSource.Play(isSpatial: true, loop: true);
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
