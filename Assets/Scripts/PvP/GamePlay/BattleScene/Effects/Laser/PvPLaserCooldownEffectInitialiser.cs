using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Projectiles.Spawners.Beams.Laser;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public class PvPLaserCooldownEffectInitialiser : MonoBehaviour, IPvPLaserCooldownEffectInitialiser
    {
        public float laserStoppedDebounceTimeInS = 0.5f;

        public IManagedDisposable CreateLaserCooldownEffect(ILaserEmitter laserEmitter)
        {
            Assert.IsNotNull(laserEmitter);

            PvPLaserFlapController laserFlap = GetComponentInChildren<PvPLaserFlapController>();
            Assert.IsNotNull(laserFlap);
            laserFlap.Initialise();

            IPvPParticleSystemGroupInitialiser smokeDischargeInitialiser = transform.FindNamedComponent<IPvPParticleSystemGroupInitialiser>("SmokeDischarge");
            IPvPParticleSystemGroup smokeDischarge = smokeDischargeInitialiser.CreateParticleSystemGroup();

            return
                new PvPLaserCooldownEffect(
                    laserEmitter.IsLaserFiring,
                    laserFlap,
                    smokeDischarge,
                    new Debouncer(PvPTimeBC.Instance.TimeSinceGameStartProvider, laserStoppedDebounceTimeInS));
        }
    }
}