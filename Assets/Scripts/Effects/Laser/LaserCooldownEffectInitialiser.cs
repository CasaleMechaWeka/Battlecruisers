using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Projectiles.Spawners.Laser;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Laser
{
    public class LaserCooldownEffectInitialiser : MonoBehaviour, ILaserCooldownEffectInitialiser
    {
        public float laserStoppedDebounceTimeInS = 0.5f;

        public IManagedDisposable CreateLaserCooldownEffect(ILaserEmitter laserEmitter)
        {
            Assert.IsNotNull(laserEmitter);

            LaserFlapController laserFlap = GetComponentInChildren<LaserFlapController>();
            Assert.IsNotNull(laserFlap);
            laserFlap.Initialise();

            IParticleSystemGroupInitialiser smokeDischargeInitialiser = transform.FindNamedComponent<IParticleSystemGroupInitialiser>("SmokeDischarge");
            IParticleSystemGroup smokeDischarge = smokeDischargeInitialiser.CreateParticleSystemGroup();

            return 
                new LaserCooldownEffect(
                    laserEmitter.IsLaserFiring, 
                    laserFlap, 
                    smokeDischarge,
                    new Debouncer(TimeBC.Instance.TimeSinceGameStartProvider, laserStoppedDebounceTimeInS));
        }
    }
}