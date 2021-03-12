using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Projectiles.Spawners
{
    public interface IProjectileSpawnerArgs
    {
        ITarget Parent { get; }
        IProjectileStats ProjectileStats { get; }
        int BurstSize { get; }
        IFactoryProvider FactoryProvider { get; }
        ICruiserSpecificFactories CruiserSpecificFactories { get; }
        ICruiser EnempCruiser { get; }
    }
}
