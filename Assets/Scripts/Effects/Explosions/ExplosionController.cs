using BattleCruisers.Effects.ParticleSystems;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionController : ParticleSystemGroupInitialiser
    {
        public virtual IExplosion Initialise()
        {
            return 
                new Explosion(
                    this, 
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }
    }
}