using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPProjectileSpawnerArgs : IPvPProjectileSpawnerArgs
    {
        public ITarget Parent { get; }
        public IProjectileStats ProjectileStats { get; }
        public int BurstSize { get; }
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public IPvPCruiser EnempCruiser { get; }

        public PvPProjectileSpawnerArgs(
            IPvPBarrelControllerArgs barrelControllerArgs,
            IProjectileStats projectileStats,
            int burstSize)
            : this(
                  barrelControllerArgs.Parent,
                  projectileStats,
                  burstSize,
                  barrelControllerArgs.CruiserSpecificFactories,
                  barrelControllerArgs.EnemyCruiser)
        { }

        public PvPProjectileSpawnerArgs(
            ITarget parent,
            IProjectileStats projectileStats,
            int burstSize,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            IPvPCruiser enemyCruiser)
        {
            PvPHelper.AssertIsNotNull(parent, projectileStats, cruiserSpecificFactories, enemyCruiser);

            Parent = parent;
            ProjectileStats = projectileStats;
            BurstSize = burstSize;
            CruiserSpecificFactories = cruiserSpecificFactories;
            EnempCruiser = enemyCruiser;
        }
    }
}
