using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class FireAndSmokeExplosionController : ExplosionController
    {
        protected override IBroadcastingParticleSystem[] GetParticleSystems()
        {
            SynchronizedParticleSystemsController[] synchronisedParticleSystems = GetComponentsInChildren<SynchronizedParticleSystemsController>();
            Assert.IsTrue(synchronisedParticleSystems.Length != 0);

            foreach (SynchronizedParticleSystemsController synchronisedSystem in synchronisedParticleSystems)
            {
                synchronisedSystem.Initialise(RandomGenerator.Instance);
            }

            return base.GetParticleSystems();
        }
    }
}