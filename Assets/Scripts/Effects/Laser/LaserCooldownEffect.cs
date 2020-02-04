using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Effects.Laser
{
    // FELIX  use, test :)
    public class LaserCooldownEffect : IManagedDisposable
    {
        private readonly IFireIntervalManager _fireIntervalManager;
        private readonly ILaserFlap _laserFlap;
        private readonly IParticleSystemGroup _overheatingSmoke;

        public LaserCooldownEffect(IFireIntervalManager fireIntervalManager, ILaserFlap laserFlap, IParticleSystemGroup overheatingSmoke)
        {
            Helper.AssertIsNotNull(fireIntervalManager, laserFlap, overheatingSmoke);

            _fireIntervalManager = fireIntervalManager;
            _laserFlap = laserFlap;
            _overheatingSmoke = overheatingSmoke;

            _fireIntervalManager.ShouldFire.ValueChanged += ShouldFire_ValueChanged;

            PlayEffects();
        }

        private void ShouldFire_ValueChanged(object sender, EventArgs e)
        {
            PlayEffects();
        }

        private void PlayEffects()
        {
            if (_fireIntervalManager.ShouldFire.Value)
            {
                _laserFlap.CloseFlap();
            }
            else
            {
                _laserFlap.OpenFlap();
                _overheatingSmoke.Play();
            }
        }

        public void DisposeManagedState()
        {
            _fireIntervalManager.ShouldFire.ValueChanged -= ShouldFire_ValueChanged;
        }
    }
}