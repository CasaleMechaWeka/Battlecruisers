using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ProjectileSpawnerArgs : IProjectileSpawnerArgs
    {
        public ITarget Parent { get; }
        public IProjectileStats ProjectileStats { get; }
        public int BurstSize { get; }
        public IFactoryProvider FactoryProvider { get; }

        public ProjectileSpawnerArgs(
            ITarget parent, 
            IProjectileStats projectileStats, 
            int burstSize,
            IFactoryProvider factoryProvider)
        {
            Helper.AssertIsNotNull(parent, projectileStats, factoryProvider);

            Parent = parent;
            ProjectileStats = projectileStats;
            BurstSize = burstSize;
            FactoryProvider = factoryProvider;
        }
    }
}
