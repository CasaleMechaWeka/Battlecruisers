using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class AdvancedExplosion : MonoBehaviour
    {
        public void Initialise(IRandomGenerator randomGenerator)
        {
            Assert.IsNotNull(randomGenerator);

            SynchronizedParticleSystemsController[] fireSmokePairs = GetComponentsInChildren<SynchronizedParticleSystemsController>(includeInactive: true);

            foreach (SynchronizedParticleSystemsController fireSmokePair in fireSmokePairs)
            {
                fireSmokePair.Initialise(randomGenerator);
            }
        }
    }
}