using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class AdvancedExplosion : MonoBehaviour
    {
        public void Initialise()
        {
            SynchronizedParticleSystemsController[] fireSmokePairs = FindObjectsOfType<SynchronizedParticleSystemsController>();

            foreach (SynchronizedParticleSystemsController fireSmokePair in fireSmokePairs)
            {
                fireSmokePair.Initialise();
            }
        }
    }
}