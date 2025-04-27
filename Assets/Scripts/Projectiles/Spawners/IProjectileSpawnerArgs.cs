using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Projectiles.Spawners
{
    public interface IProjectileSpawnerArgs
    {
        ITarget Parent { get; }
        ProjectileStats ProjectileStats { get; }
        int BurstSize { get; }
        CruiserSpecificFactories CruiserSpecificFactories { get; }
        ICruiser EnempCruiser { get; }
    }
}
