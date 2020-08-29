using BattleCruisers.Effects.ParticleSystems;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionController : ParticleSystemGroupInitialiser
    {
        public virtual IExplosion Initialise()
        {
            IBroadcastingParticleSystem[] particleSystems = GetParticleSystems();
            return 
                new Explosion(
                    this, 
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }
    }
}