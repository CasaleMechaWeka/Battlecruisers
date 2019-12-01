using UnityEngine.Assertions;

namespace BattleCruisers.Effects
{
    // FELIX  Interface
    public class ParticleSystemGroup
    { 
        protected readonly IBroadcastingParticleSystem[] _particleSystems;

        public ParticleSystemGroup(IBroadcastingParticleSystem[] particleSystems)
        {
            Assert.IsNotNull(particleSystems);
            Assert.IsTrue(particleSystems.Length != 0);

            _particleSystems = particleSystems;
        }

        public void Play()
        {
            foreach (IBroadcastingParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
        }
    }
}