using BattleCruisers.Effects.Explosions;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.ParticleSystems
{
    public class ParticleSystemGroupInitialiser : MonoBehaviourWrapper, IParticleSystemGroupInitialiser
    {
        public IParticleSystemGroup CreateParticleSystemGroup()
        {
            return
                new ParticleSystemGroup(
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }

        protected virtual IBroadcastingParticleSystem[] GetParticleSystems()
        {
            BroadcastingParticleSystem[] particleSystems = GetComponentsInChildren<BroadcastingParticleSystem>();
            Assert.IsTrue(particleSystems.Length != 0);

            foreach (BroadcastingParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Initialise();
            }

            return particleSystems;
        }

        protected ISynchronizedParticleSystems[] GetSynchronizedSystems()
        {
            SynchronizedParticleSystemsController[] synchronizedSystems = GetComponentsInChildren<SynchronizedParticleSystemsController>();

            foreach (SynchronizedParticleSystemsController system in synchronizedSystems)
            {
                system.Initialise();
            }

            return synchronizedSystems;
        }
    }
}