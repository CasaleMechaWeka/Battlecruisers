namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionStats : IExplosionStats
    {
        public ExplosionSize Size { get; }
        public bool ShowTrails { get; }

        public ExplosionStats(ExplosionSize size, bool showTrails)
        {
            Size = size;
            ShowTrails = showTrails;
        }
    }
}