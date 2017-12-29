namespace BattleCruisers.Projectiles.Explosions
{
    public class BasicExplosion : Explosion
    {
        protected override void OnShow()
        {
            Destroy(gameObject, _durationInS);
        }
    }
}
