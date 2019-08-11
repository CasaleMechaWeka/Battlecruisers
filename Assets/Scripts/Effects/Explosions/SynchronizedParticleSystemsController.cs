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

            BroadcastingParticleSystem[] particleSystems = GetComponentsInChildren<BroadcastingParticleSystem>(includeInactive: true);

            foreach (BroadcastingParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Initialise();
                particleSystem.ParticleSystem.randomSeed = (uint)seed;
            }
        }
    }
}