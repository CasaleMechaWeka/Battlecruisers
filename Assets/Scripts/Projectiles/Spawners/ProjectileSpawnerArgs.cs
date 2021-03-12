using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Cruisers;
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
        public ICruiserSpecificFactories CruiserSpecificFactories { get; }
        public ICruiser EnempCruiser { get; }

        public ProjectileSpawnerArgs(
            IBarrelControllerArgs barrelControllerArgs,
            IProjectileStats projectileStats,
            int burstSize)
            : this(
                  barrelControllerArgs.Parent,
                  projectileStats,
                  burstSize,
                  barrelControllerArgs.FactoryProvider,
                  barrelControllerArgs.CruiserSpecificFactories,
                  barrelControllerArgs.EnemyCruiser)
        { }

        public ProjectileSpawnerArgs(
            ITarget parent,
            IProjectileStats projectileStats, 
            int burstSize,
            IFactoryProvider factoryProvider,
            ICruiserSpecificFactories cruiserSpecificFactories,
            ICruiser enemyCruiser)
        {
            Helper.AssertIsNotNull(parent, projectileStats, factoryProvider, cruiserSpecificFactories, enemyCruiser);

            Parent = parent;
            ProjectileStats = projectileStats;
            BurstSize = burstSize;
            FactoryProvider = factoryProvider;
            CruiserSpecificFactories = cruiserSpecificFactories;
            EnempCruiser = enemyCruiser;
        }
    }
}
