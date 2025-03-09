using BattleCruisers.Effects.Laser;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public class PvPLaserCooldownEffectInitialiser : MonoBehaviour, ILaserCooldownEffectInitialiser
    {
        public float laserStoppedDebounceTimeInS = 0.5f;

        public IManagedDisposable CreateLaserCooldownEffect(ILaserEmitter laserEmitter)
        {
            Assert.IsNotNull(laserEmitter);

            PvPLaserFlapController laserFlap = GetComponentInChildren<PvPLaserFlapController>();
            Assert.IsNotNull(laserFlap);
            laserFlap.Initialise();

            IParticleSystemGroupInitialiser smokeDischargeInitialiser = transform.FindNamedComponent<IParticleSystemGroupInitialiser>("SmokeDischarge");
            IParticleSystemGroup smokeDischarge = smokeDischargeInitialiser.CreateParticleSystemGroup();

            return
                new PvPLaserCooldownEffect(
                    laserEmitter.IsLaserFiring,
                    laserFlap,
                    smokeDischarge,
                    new Debouncer(PvPTimeBC.Instance.TimeSinceGameStartProvider, laserStoppedDebounceTimeInS));
        }
    }
}