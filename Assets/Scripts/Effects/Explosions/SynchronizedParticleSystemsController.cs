using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class SynchronizedParticleSystemsController : MonoBehaviour
    {
        void Awake()
        {
            Logging.Verbose(Tags.EXPLOSIONS, ToString());

            int seed = RandomGenerator.Instance.Range(0, int.MaxValue);

            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);

            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.randomSeed = (uint)seed;
            }
        }
    }
}