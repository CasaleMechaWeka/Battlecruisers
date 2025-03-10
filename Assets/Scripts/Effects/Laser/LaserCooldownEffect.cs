using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using System;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Effects.Laser
{
    public class LaserCooldownEffect : IManagedDisposable
    {
        private readonly IBroadcastingProperty<bool> _isLaserFiring;
        private readonly ILaserFlap _laserFlap;
        private readonly IParticleSystemGroup _overheatingSmoke;
        private readonly IDebouncer _laserStoppdDebouncer;

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

        public LaserCooldownEffect(
            IBroadcastingProperty<bool> isLaserFiring, 
            ILaserFlap laserFlap, 
            IParticleSystemGroup overheatingSmoke,
            IDebouncer laserStoppedDebouncer)
        {
            Helper.AssertIsNotNull(isLaserFiring, laserFlap, overheatingSmoke, laserStoppedDebouncer);

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