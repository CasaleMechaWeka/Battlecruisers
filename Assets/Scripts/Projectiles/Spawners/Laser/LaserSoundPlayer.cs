using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Projectiles.Spawners.Laser
{
    public class LaserSoundPlayer : ILaserSoundPlayer
    {
        private readonly ILaserRenderer _laserRenderer;
        private readonly IAudioSourceWrapper _audioSource;

        public LaserSoundPlayer(ILaserRenderer laserRenderer, IAudioSourceWrapper audioSource, ISoundFetcher soundFetcher)
        {
            Helper.AssertIsNotNull(laserRenderer, audioSource, soundFetcher);

            _laserRenderer = laserRenderer;
            _audioSource = audioSource;

            _laserRenderer.LaserVisibilityChanged += _laserRenderer_LaserVisibilityChanged;

            IAudioClipWrapper laserSound = soundFetcher.GetSound(SoundKeys.Firing.Laser);
            _audioSource.AudioClip = laserSound;
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
