using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.ParticleSystems
{
    public class ParticleSystemGroup : IParticleSystemGroup
    {
        protected readonly IBroadcastingParticleSystem[] _particleSystems;
        private readonly ISynchronizedParticleSystems[] _synchronizedSystems;

        public ParticleSystemGroup(IBroadcastingParticleSystem[] particleSystems, ISynchronizedParticleSystems[] synchronizedSystems)
        {
            Helper.AssertIsNotNull(particleSystems, synchronizedSystems);
            Assert.IsTrue(particleSystems.Length != 0);
            // Synchronized systems may be empty

            _particleSystems = particleSystems;
            _synchronizedSystems = synchronizedSystems;
        }

        public void Play()
        {
            foreach (ISynchronizedParticleSystems system in _synchronizedSystems)
            {
                system.ResetSeed();
            }

            foreach (IBroadcastingParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
        }

        public void Stop()
        {
            foreach (IBroadcastingParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.Stop();
            }
        }
    }
}