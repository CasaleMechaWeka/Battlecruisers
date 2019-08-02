namespace BattleCruisers.Effects.Explosions
{
    public enum ExplosionSize
    {
        Small,  // Radius 0.75m => Bomber, mortar, SAM site
        Medium, // Radius 1m    => Destroyer missiles, archon front launcher missiles
        Large,  // Radius 1.5m  => Artillery, rocket launcher, broadsides, archon back missile launcher, archon primary cannon
        Giant   // Radius 5m    => Nuke
    }

    public interface IExplosionStats
    {
        ExplosionSize Size { get; }

        // PERF  This is currently unused.  May need to start using it to reduce explosion
        // complexity for performance.
        bool ShowTrails { get; }
    }
}