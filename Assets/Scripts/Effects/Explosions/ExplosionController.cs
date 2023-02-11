using BattleCruisers.Effects.ParticleSystems;
using UnityEngine;
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