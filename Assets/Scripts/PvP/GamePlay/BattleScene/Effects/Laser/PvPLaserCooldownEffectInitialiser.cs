using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public class PvPLaserCooldownEffectInitialiser : MonoBehaviour, IPvPLaserCooldownEffectInitialiser
    {
        public float laserStoppedDebounceTimeInS = 0.5f;

        public IPvPManagedDisposable CreateLaserCooldownEffect(IPvPLaserEmitter laserEmitter)
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
                    new PvPDebouncer(PvPTimeBC.Instance.TimeSinceGameStartProvider, laserStoppedDebounceTimeInS));
        }
    }
}