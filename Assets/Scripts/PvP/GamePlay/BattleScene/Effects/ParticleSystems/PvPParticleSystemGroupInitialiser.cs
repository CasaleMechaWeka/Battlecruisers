using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using Unity.Netcode;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public class PvPParticleSystemGroupInitialiser : PvPMonoBehaviourWrapper, IPvPParticleSystemGroupInitialiser
    {
        public IPvPParticleSystemGroup CreateParticleSystemGroup()
        {
            return
                new PvPParticleSystemGroup(
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }

        protected virtual IPvPBroadcastingParticleSystem[] GetParticleSystems()
        {
            PvPBroadcastingParticleSystem[] particleSystems = GetComponentsInChildren<PvPBroadcastingParticleSystem>(includeInactive: true);
            Assert.IsTrue(particleSystems.Length != 0);

            foreach (PvPBroadcastingParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Initialise();
            }

            return particleSystems;
        }

        protected IPvPSynchronizedParticleSystems[] GetSynchronizedSystems()
        {
            PvPSynchronizedParticleSystemsController[] synchronizedSystems = GetComponentsInChildren<PvPSynchronizedParticleSystemsController>();

            foreach (PvPSynchronizedParticleSystemsController system in synchronizedSystems)
            {
                system.Initialise();
            }

            return synchronizedSystems;
        }




    }
}