using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPProjectileSpawnerArgs
    {
        public ITarget Parent { get; }
        public ProjectileStats ProjectileStats { get; }
        public int BurstSize { get; }
        public PvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public IPvPCruiser EnempCruiser { get; }

        public PvPProjectileSpawnerArgs(
            PvPBarrelControllerArgs barrelControllerArgs,
            ProjectileStats projectileStats,
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
            ProjectileStats projectileStats,
            int burstSize,
            PvPCruiserSpecificFactories cruiserSpecificFactories,
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
