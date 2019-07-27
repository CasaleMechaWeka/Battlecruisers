using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class SynchronizedParticleSystemsController : MonoBehaviour
    {
        public void Initialise()
        {
            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.randomSeed = particleSystems[0].randomSeed;
            }
        }
    }
}