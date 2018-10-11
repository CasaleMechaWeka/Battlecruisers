namespace BattleCruisers.Effects.Explosions
{
    public interface IExplosionFactory
    {
        IExplosion CreateDummyExplosion();
        IExplosion CreateExplosion(float damageRadiusInM, float durationInS = ExplosionFactory.DEFAULT_EXPLOSION_DURATION_IN_S);
    }
}
