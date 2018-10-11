namespace BattleCruisers.Effects.Explosions
{
    // TEMP  Remove now that we have FragExplosions?  Keep for now, in case FragExplosion performance is abysmal :P
    public class BasicExplosion : Explosion
    {
        protected override void OnShow()
        {
            Destroy(gameObject, _durationInS);
        }
    }
}
