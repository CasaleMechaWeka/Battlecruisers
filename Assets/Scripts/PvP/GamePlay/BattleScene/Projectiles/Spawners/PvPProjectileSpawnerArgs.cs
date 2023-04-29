using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPProjectileSpawnerArgs : IPvPProjectileSpawnerArgs
    {
        public IPvPTarget Parent { get; }
        public IPvPProjectileStats ProjectileStats { get; }
        public int BurstSize { get; }
        public IPvPFactoryProvider FactoryProvider { get; }
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public IPvPCruiser EnempCruiser { get; }

        public PvPProjectileSpawnerArgs(
            IPvPBarrelControllerArgs barrelControllerArgs,
            IPvPProjectileStats projectileStats,
            int burstSize)
            : this(
                  barrelControllerArgs.Parent,
                  projectileStats,
                  burstSize,
                  barrelControllerArgs.FactoryProvider,
                  barrelControllerArgs.CruiserSpecificFactories,
                  barrelControllerArgs.EnemyCruiser)
        { }

        public PvPProjectileSpawnerArgs(
            IPvPTarget parent,
            IPvPProjectileStats projectileStats,
            int burstSize,
            IPvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            IPvPCruiser enemyCruiser)
        {
            PvPHelper.AssertIsNotNull(parent, projectileStats, factoryProvider, cruiserSpecificFactories, enemyCruiser);

            Parent = parent;
            ProjectileStats = projectileStats;
            BurstSize = burstSize;
            FactoryProvider = factoryProvider;
            CruiserSpecificFactories = cruiserSpecificFactories;
            EnempCruiser = enemyCruiser;
        }
    }
}
