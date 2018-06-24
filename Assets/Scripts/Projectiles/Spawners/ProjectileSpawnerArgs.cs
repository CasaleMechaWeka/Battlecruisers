using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Utils;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ProjectileSpawnerArgs : IProjectileSpawnerArgs
    {
        public ITarget Parent { get; private set; }
        public IProjectileStats ProjectileStats { get; private set; }
        public int BurstSize { get; private set; }
        public IFactoryProvider FactoryProvider { get; private set; }

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
