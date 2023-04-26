using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using UnityEngine;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions
{
    public class PvPExplosionController : PvPParticleSystemGroupInitialiser
    {
        public virtual IPvPExplosion Initialise()
        {
            return
                new PvPExplosion(
                    this,
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }
    }

}
