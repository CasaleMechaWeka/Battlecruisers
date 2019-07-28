using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class SynchronizedParticleSystemsController : MonoBehaviour
    {
        public void Initialise(IRandomGenerator randomGenerator)
        {
            Assert.IsNotNull(randomGenerator);

            int seed = randomGenerator.Range(0, int.MaxValue);

            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);

            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.randomSeed = (uint)seed;
            }
        }
    }
}