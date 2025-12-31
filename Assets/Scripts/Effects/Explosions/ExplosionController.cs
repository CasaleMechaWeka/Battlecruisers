using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionController : ParticleSystemGroupInitialiser
    {

        public virtual IPoolable<Vector3> Initialise()
        {

            return
                new Explosion(
                    this,
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }

    }

}