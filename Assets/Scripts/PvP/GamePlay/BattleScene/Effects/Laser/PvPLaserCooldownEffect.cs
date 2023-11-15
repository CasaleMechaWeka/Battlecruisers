using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public class PvPLaserCooldownEffect : IPvPManagedDisposable
    {
        private readonly IPvPBroadcastingProperty<bool> _isLaserFiring;
        private readonly IPvPLaserFlap _laserFlap;
        private readonly IPvPParticleSystemGroup _overheatingSmoke;
        private readonly IPvPDebouncer _laserStoppdDebouncer;

        private bool _laserIsActive;
        private bool LaserIsActive
        {
            get => _laserIsActive;
            set
            {
                _laserIsActive = value;

                if (_laserIsActive)
                {
                    _laserFlap.CloseFlap();
                }
                else
                {
                    _laserFlap.OpenFlap();
                    _overheatingSmoke.Play();
                }
            }
        }

        public PvPLaserCooldownEffect(
            IPvPBroadcastingProperty<bool> isLaserFiring,
            IPvPLaserFlap laserFlap,
            IPvPParticleSystemGroup overheatingSmoke,
            IPvPDebouncer laserStoppedDebouncer)
        {
            PvPHelper.AssertIsNotNull(isLaserFiring, laserFlap, overheatingSmoke, laserStoppedDebouncer);

            _isLaserFiring = isLaserFiring;
            _laserFlap = laserFlap;
            _overheatingSmoke = overheatingSmoke;
            _laserStoppdDebouncer = laserStoppedDebouncer;

            _isLaserFiring.ValueChanged += _isLaserFiring_ValueChanged;
        }

        private void _isLaserFiring_ValueChanged(object sender, EventArgs e)
        {
            if (_isLaserFiring.Value
                && !LaserIsActive)
            {
                LaserIsActive = true;
            }
            else if (!_isLaserFiring.Value
                && LaserIsActive)
            {
                _laserStoppdDebouncer.Debounce(LaserStopped);
            }
        }

        private void LaserStopped()
        {
            LaserIsActive = false;
        }

        public void DisposeManagedState()
        {
            _isLaserFiring.ValueChanged -= _isLaserFiring_ValueChanged;
        }
    }
}