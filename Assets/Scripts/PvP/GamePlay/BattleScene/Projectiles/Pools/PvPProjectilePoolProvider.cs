using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Projectiles.Stats;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPProjectilePoolProvider : IPvPProjectilePoolProvider
    {
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> HighCalibreBulletsPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> TinyBulletsPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> FlakBulletsPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> NovaShellPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> FiveShellCluster { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> RocketShellPool { get; }
        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        public IPool<PvPBombController, PvPProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        public IPool<PvPBombController, PvPProjectileActivationArgs<IProjectileStats>> StratBombsPool { get; }
        public IPool<PvPRocketController, PvPTargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        public IPool<PvPRocketController, PvPTargetProviderActivationArgs<ICruisingProjectileStats>> RocketsSmallPool { get; }
        public IPool<PvPRocketController, PvPTargetProviderActivationArgs<ICruisingProjectileStats>> MissilesFirecrackerPool { get; }
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>> MissilesMFPool { get; }
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
        public IPool<PvPSmartMissileController, PvPSmartMissileActivationArgs<ISmartProjectileStats>> MissilesSmartPool { get; }

        public PvPProjectilePoolProvider(IPvPFactoryProvider factoryProvider)
        {
            Assert.IsNotNull(factoryProvider);

            BulletsPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPBullet,
                    PvPInitialCapacity.BULLET);

            HighCalibreBulletsPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPHighCalibreBullet,
                    PvPInitialCapacity.BULLET);

            TinyBulletsPool
               = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                   factoryProvider,
                   PvPStaticPrefabKeys.PvPProjectiles.PvPTinyBullet,
                   PvPInitialCapacity.BULLET);

            FlakBulletsPool
               = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                   factoryProvider,
                   PvPStaticPrefabKeys.PvPProjectiles.PvPFlakBullet,
                   PvPInitialCapacity.BULLET);

            ShellsSmallPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPShellSmall,
                    PvPInitialCapacity.SHELL_SMALL);

            ShellsLargePool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPShellLarge,
                    PvPInitialCapacity.SHELL_LARGE);

            NovaShellPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPNovaShell,
                    PvPInitialCapacity.SHELL_LARGE);

            FiveShellCluster
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPFiveShellCluster,
                    PvPInitialCapacity.SHELL_LARGE);

            RocketShellPool
                = CreatePool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRocketShell,
                    PvPInitialCapacity.SHELL_LARGE);


            BombsPool
                = CreatePool<PvPBombController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPBomb,
                    PvPInitialCapacity.BOMB);


            StratBombsPool
                = CreatePool<PvPBombController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPStratBomb,
                    PvPInitialCapacity.BOMB);

            RocketsPool
                = CreatePool<PvPRocketController, PvPTargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRocket,
                    PvPInitialCapacity.ROCKET);

            RocketsSmallPool
                = CreatePool<PvPRocketController, PvPTargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRocketSmall,
                    PvPInitialCapacity.ROCKET);

            MissilesSmallPool
                = CreatePool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileSmall,
                    PvPInitialCapacity.MISSILE_SMALL);

            MissilesMediumPool
                = CreatePool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileMedium,
                    PvPInitialCapacity.MISSILE_MEDIUM);

            MissilesMFPool
                = CreatePool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileMF,
                    PvPInitialCapacity.MISSILE_MEDIUM);

            MissilesFirecrackerPool
                = CreatePool<PvPRocketController, PvPTargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileFirecracker,
                    PvPInitialCapacity.MISSILE_MEDIUM);

            MissilesLargePool
                = CreatePool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileLarge,
                    PvPInitialCapacity.MISSILE_LARGE);

            MissilesSmartPool
                = CreatePool<PvPSmartMissileController, PvPSmartMissileActivationArgs<ISmartProjectileStats>, ISmartProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileSmart,
                    PvPInitialCapacity.MISSILE_SMART);


        }

        private IPool<TProjectile, TArgs> CreatePool<TProjectile, TArgs, TStats>(IPvPFactoryProvider factoryProvider, PvPProjectileKey projectileKey, int initialCapacity)
            where TArgs : PvPProjectileActivationArgs<TStats>
            where TProjectile : PvPProjectileControllerBase<TArgs, TStats>
            where TStats : IProjectileStats
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