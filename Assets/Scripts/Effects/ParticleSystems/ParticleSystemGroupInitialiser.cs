using UnityEngine.Assertions;

namespace BattleCruisers.Effects.ParticleSystems
{
    public class ParticleSystemGroupInitialiser : MonoBehaviourWrapper, IParticleSystemGroupInitialiser
    {
        public IParticleSystemGroup CreateParticleSystemGroup()
        {
            IBroadcastingParticleSystem[] particleSystems = GetParticleSystems();
            return new ParticleSystemGroup(particleSystems);
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
    }
}