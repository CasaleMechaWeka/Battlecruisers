using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public interface IExplosionManager
    {
        void ShowExplosion(IExplosionStats explosionStats, Vector2 position);
    }
}