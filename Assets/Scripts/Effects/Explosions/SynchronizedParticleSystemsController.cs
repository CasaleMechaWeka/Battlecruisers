using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class SynchronizedParticleSystemsController : MonoBehaviour, ISynchronizedParticleSystems
    {
        private ParticleSystem[] _particleSystems;

        public void Initialise()
        {
            Logging.Verbose(Tags.EXPLOSIONS, ToString());
            _particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
        }

        public void ResetSeed()
        {
            int seed = RandomGenerator.Instance.Range(0, int.MaxValue);

            Logging.Verbose(Tags.EXPLOSIONS, $"{ToString()}  seed: {seed}");

            foreach (ParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.randomSeed = (uint)seed;
            }
        }
    }
}