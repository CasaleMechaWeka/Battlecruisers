using UnityEngine;

namespace BattleCruisers.Effects
{
    public class DummyParticleSystemGroupInitialiser : MonoBehaviour, IParticleSystemGroupInitialiser
    {
        public IParticleSystemGroup CreateParticleSystemGroup()
        {
            return new DummyParticleSystemGroup();
        }
    }
}