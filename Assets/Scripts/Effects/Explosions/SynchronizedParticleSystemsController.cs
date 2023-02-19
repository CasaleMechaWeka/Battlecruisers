using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class SynchronizedParticleSystemsController : MonoBehaviour, ISynchronizedParticleSystems
    {
        private static int _randomSeed;
        private ParticleSystem[] _particleSystems;

        public void Initialise()
        {
            Logging.Verbose(Tags.EXPLOSIONS, ToString());
            _particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);

            if (_randomSeed == 0)
            {
                _randomSeed = RandomGenerator.Instance.Range(0, int.MaxValue);
            }

            Logging.Verbose(Tags.EXPLOSIONS, $"{ToString()}  seed: {_randomSeed}");

            foreach (ParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.randomSeed = (uint)_randomSeed;
            }
        }

        public void ResetSeed()
        {
            _randomSeed = RandomGenerator.Instance.Range(0, int.MaxValue);
            Initialise();
        }
    }
}
