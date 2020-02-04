using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Laser
{
    public class LaserCooldownEffectInitialiser : MonoBehaviour
    {
        public IManagedDisposable CreateLaserCooldownEffect(IFireIntervalManager fireIntervalManager)
        {
            Assert.IsNotNull(fireIntervalManager);

            LaserFlapController laserFlap = GetComponentInChildren<LaserFlapController>();
            Assert.IsNotNull(laserFlap);
            laserFlap.Initialise();

            IParticleSystemGroupInitialiser smokeDischargeInitialiser = transform.FindNamedComponent<IParticleSystemGroupInitialiser>("SmokeDischarge");
            IParticleSystemGroup smokeDischarge = smokeDischargeInitialiser.CreateParticleSystemGroup();

            return new LaserCooldownEffect(fireIntervalManager, laserFlap, smokeDischarge);
        }
    }
}