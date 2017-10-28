using UnityEngine;

namespace BattleCruisers.Projectiles.Explosions
{
    public interface IExplosionFactory
    {
        IExplosion CreateDummyExplosion();

        IExplosion CreateExplosion(
            Transform parent, 
            float radiusInM, 
            float durationInS = ExplosionFactory.DEFAULT_EXPLOSION_DURATION_IN_S);
    }
}
