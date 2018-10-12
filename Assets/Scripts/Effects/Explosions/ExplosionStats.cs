namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionStats : IExplosionStats
    {
        public ExplosionSize Size { get; private set; }
        public bool ShowTrails { get; private set; }

        public ExplosionStats(ExplosionSize size, bool showTrails)
        {
            Size = size;
            ShowTrails = showTrails;
        }
    }
}