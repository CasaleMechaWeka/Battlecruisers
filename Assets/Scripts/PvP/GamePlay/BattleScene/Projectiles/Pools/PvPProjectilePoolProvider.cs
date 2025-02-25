using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPProjectilePoolProvider : IPvPProjectilePoolProvider
    {
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> BulletsPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> HighCalibreBulletsPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> TinyBulletsPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> FlakBulletsPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> ShellsLargePool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> NovaShellPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> FiveShellCluster { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> RocketShellPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> ShellsSmallPool { get; }
        public IPool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>> BombsPool { get; }
        public IPool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>> StratBombsPool { get; }
        public IPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsPool { get; }
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesSmallPool { get; }
        public IPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsSmallPool { get; }
        public IPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> MissilesFirecrackerPool { get; }
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesMediumPool { get; }
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesMFPool { get; }
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesLargePool { get; }
        public IPool<PvPSmartMissileController, PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>> MissilesSmartPool { get; }

        public PvPProjectilePoolProvider(IPvPFactoryProvider factoryProvider)
        {
            Assert.IsNotNull(factoryProvider);

            BulletsPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPBullet,
                    PvPInitialCapacity.BULLET);

            HighCalibreBulletsPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPHighCalibreBullet,
                    PvPInitialCapacity.BULLET);

            TinyBulletsPool
               = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                   factoryProvider,
                   PvPStaticPrefabKeys.PvPProjectiles.PvPTinyBullet,
                   PvPInitialCapacity.BULLET);

            FlakBulletsPool
               = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                   factoryProvider,
                   PvPStaticPrefabKeys.PvPProjectiles.PvPFlakBullet,
                   PvPInitialCapacity.BULLET);

            ShellsSmallPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPShellSmall,
                    PvPInitialCapacity.SHELL_SMALL);

            ShellsLargePool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPShellLarge,
                    PvPInitialCapacity.SHELL_LARGE);

            NovaShellPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPNovaShell,
                    PvPInitialCapacity.SHELL_LARGE);

            FiveShellCluster
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPFiveShellCluster,
                    PvPInitialCapacity.SHELL_LARGE);

            RocketShellPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRocketShell,
                    PvPInitialCapacity.SHELL_LARGE);


            BombsPool
                = CreatePool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPBomb,
                    PvPInitialCapacity.BOMB);


            StratBombsPool
                = CreatePool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPStratBomb,
                    PvPInitialCapacity.BOMB);

            RocketsPool
                = CreatePool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>, IPvPCruisingProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRocket,
                    PvPInitialCapacity.ROCKET);

            RocketsSmallPool
                = CreatePool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>, IPvPCruisingProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRocketSmall,
                    PvPInitialCapacity.ROCKET);

            MissilesSmallPool
                = CreatePool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileSmall,
                    PvPInitialCapacity.MISSILE_SMALL);

            MissilesMediumPool
                = CreatePool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileMedium,
                    PvPInitialCapacity.MISSILE_MEDIUM);

            MissilesMFPool
                = CreatePool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileMF,
                    PvPInitialCapacity.MISSILE_MEDIUM);

            MissilesFirecrackerPool
                = CreatePool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>, IPvPCruisingProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileFirecracker,
                    PvPInitialCapacity.MISSILE_MEDIUM);

            MissilesLargePool
                = CreatePool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileLarge,
                    PvPInitialCapacity.MISSILE_LARGE);

            MissilesSmartPool
                = CreatePool<PvPSmartMissileController, PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>, IPvPSmartProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileSmart,
                    PvPInitialCapacity.MISSILE_SMART);


        }

        private IPool<TProjectile, TArgs> CreatePool<TProjectile, TArgs, TStats>(IPvPFactoryProvider factoryProvider, PvPProjectileKey projectileKey, int initialCapacity)
            where TArgs : PvPProjectileActivationArgs<TStats>
            where TProjectile : PvPProjectileControllerBase<TArgs, TStats>
            where TStats : IPvPProjectileStats
        {
            return
                new PvPPool<TProjectile, TArgs>(
                    new PvPProjectileFactory<TProjectile, TArgs, TStats>(
                        factoryProvider,
                        projectileKey));
        }

        public void SetInitialCapacity()
        {
            BulletsPool.AddCapacity(1);
            HighCalibreBulletsPool.AddCapacity(1);
            TinyBulletsPool.AddCapacity(1);
            FlakBulletsPool.AddCapacity(1);
            ShellsSmallPool.AddCapacity(1);
            ShellsLargePool.AddCapacity(1);
            NovaShellPool.AddCapacity(1);
            FiveShellCluster.AddCapacity(1);
            RocketShellPool.AddCapacity(1);
            BombsPool.AddCapacity(1);
            StratBombsPool.AddCapacity(1);
            RocketsPool.AddCapacity(1);
            RocketsSmallPool.AddCapacity(1);
            MissilesSmallPool.AddCapacity(1);
            MissilesMediumPool.AddCapacity(1);
            MissilesMFPool.AddCapacity(1);
            MissilesFirecrackerPool.AddCapacity(1);
            MissilesLargePool.AddCapacity(1);
            MissilesSmartPool.AddCapacity(1);
        }

        public void SetInitialCapacity_Rest()
        {
            BulletsPool.AddCapacity(PvPInitialCapacity.BULLET - 1);
            HighCalibreBulletsPool.AddCapacity(PvPInitialCapacity.BULLET - 1);
            TinyBulletsPool.AddCapacity(PvPInitialCapacity.BULLET - 1);
            FlakBulletsPool.AddCapacity(PvPInitialCapacity.BULLET - 1);
            ShellsSmallPool.AddCapacity(PvPInitialCapacity.SHELL_SMALL - 1);
            ShellsLargePool.AddCapacity(PvPInitialCapacity.SHELL_LARGE - 1);
            NovaShellPool.AddCapacity(PvPInitialCapacity.SHELL_LARGE - 1);
            FiveShellCluster.AddCapacity(PvPInitialCapacity.SHELL_LARGE - 1);
            RocketShellPool.AddCapacity(PvPInitialCapacity.SHELL_LARGE - 1);
            BombsPool.AddCapacity(PvPInitialCapacity.BOMB - 1);
            StratBombsPool.AddCapacity(PvPInitialCapacity.BOMB - 1);
            RocketsPool.AddCapacity(PvPInitialCapacity.ROCKET - 1);
            RocketsSmallPool.AddCapacity(PvPInitialCapacity.ROCKET - 1);
            MissilesSmallPool.AddCapacity(PvPInitialCapacity.MISSILE_SMALL - 1);
            MissilesMediumPool.AddCapacity(PvPInitialCapacity.MISSILE_MEDIUM - 1);
            MissilesMFPool.AddCapacity(PvPInitialCapacity.MISSILE_MEDIUM - 1);
            MissilesFirecrackerPool.AddCapacity(PvPInitialCapacity.MISSILE_MEDIUM - 1);
            MissilesLargePool.AddCapacity(PvPInitialCapacity.MISSILE_LARGE - 1);
            MissilesSmartPool.AddCapacity(PvPInitialCapacity.MISSILE_SMART - 1);
        }
    }
}