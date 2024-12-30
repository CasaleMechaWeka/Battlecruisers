
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser
{
    public class PvPLaserSoundPlayer : IPvPLaserSoundPlayer
    {
        private readonly IPvPLaserRenderer _laserRenderer;
        private readonly IPvPAudioSource _audioSource;

        public PvPLaserSoundPlayer(IPvPLaserRenderer laserRenderer, IPvPAudioSource audioSource)
        {
            PvPHelper.AssertIsNotNull(laserRenderer, audioSource);

            _laserRenderer = laserRenderer;
            _audioSource = audioSource;

            _laserRenderer.LaserVisibilityChanged += _laserRenderer_LaserVisibilityChanged;
        }

        private void _laserRenderer_LaserVisibilityChanged(object sender, PvPLaserVisibilityChangedEventArgs e)
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
