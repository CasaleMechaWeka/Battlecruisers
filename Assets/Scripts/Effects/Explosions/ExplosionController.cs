using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionController : MonoBehaviourWrapper
    {
        public virtual IExplosion Initialise()
        {
            IBroadcastingParticleSystem[] particleSystems = GetParticleSystems();
            return new Explosion(this, particleSystems);
        }

        protected virtual IBroadcastingParticleSystem[] GetParticleSystems()
        {
            BroadcastingParticleSystem[] particleSystems = GetComponentsInChildren<BroadcastingParticleSystem>();
            Assert.IsTrue(particleSystems.Length != 0);

            foreach (BroadcastingParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Initialise();
            }

            return particleSystems;
        }
    }
}