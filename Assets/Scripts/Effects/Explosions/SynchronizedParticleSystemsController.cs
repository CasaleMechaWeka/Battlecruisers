using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class SynchronizedParticleSystemsController : MonoBehaviour
    {
        // FELIX  Need to change random seed on every Activate() :/
        // FELIX  Check out subemitters???
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