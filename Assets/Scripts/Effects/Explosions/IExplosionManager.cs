using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    // FELIX  Remove :)
    public interface IExplosionManager
    {
        void ShowExplosion(IExplosionStats explosionStats, Vector2 position);
    }
}