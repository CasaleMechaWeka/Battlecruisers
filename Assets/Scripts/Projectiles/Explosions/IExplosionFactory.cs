using UnityEngine;

namespace BattleCruisers.Projectiles.Explosions
{
    public interface IExplosionFactory
    {
        void CreateExplosion(Transform parent, float radiusInM, float durationInS);
    }
}
