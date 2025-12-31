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
        public ProjectileStats ProjectileStats { get; }
        public int BurstSize { get; }
        public CruiserSpecificFactories CruiserSpecificFactories { get; }
        public ICruiser EnempCruiser { get; }

        public ProjectileSpawnerArgs(
            BarrelControllerArgs barrelControllerArgs,
            ProjectileStats projectileStats,
            int burstSize)
            : this(
                  barrelControllerArgs.Parent,
                  projectileStats,
                  burstSize,
                  barrelControllerArgs.CruiserSpecificFactories,
                  barrelControllerArgs.EnemyCruiser)
        { }

        public ProjectileSpawnerArgs(
            ITarget parent,
            ProjectileStats projectileStats,
            int burstSize,
            CruiserSpecificFactories cruiserSpecificFactories,
            ICruiser enemyCruiser)
        {
            Helper.AssertIsNotNull(parent, projectileStats, cruiserSpecificFactories, enemyCruiser);

            Parent = parent;
            ProjectileStats = projectileStats;
            BurstSize = burstSize;
            CruiserSpecificFactories = cruiserSpecificFactories;
            EnempCruiser = enemyCruiser;
        }
    }
}
