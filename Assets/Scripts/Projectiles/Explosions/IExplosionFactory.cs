namespace BattleCruisers.Projectiles.Explosions
{
    public interface IExplosionFactory
    {
        IExplosion CreateDummyExplosion();

        IExplosion CreateExplosion(float radiusInM, float durationInS = ExplosionFactory.DEFAULT_EXPLOSION_DURATION_IN_S);
    }
}
