using UnityEngine;

namespace BattleCruisers.Effects.ParticleSystems
{
    public class DummyParticleSystemGroupInitialiser : MonoBehaviour, IParticleSystemGroupInitialiser
    {
        public IParticleSystemGroup CreateParticleSystemGroup()
        {
            return new DummyParticleSystemGroup();
        }
    }
}