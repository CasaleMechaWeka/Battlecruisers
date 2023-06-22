using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPProjectilePoolProvider : IPvPProjectilePoolProvider
    {
        public IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> BulletsPool { get; }
        public IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> HighCalibreBulletsPool { get; }
        public IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> TinyBulletsPool { get; }
        public IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> ShellsLargePool { get; }

        public IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> NovaShellPool { get; }

        public IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> RocketShellPool { get; }
        public IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> ShellsSmallPool { get; }
        public IPvPPool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>> BombsPool { get; }
        public IPvPPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsPool { get; }
        public IPvPPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesSmallPool { get; }
        public IPvPPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsSmallPool { get; }
        public IPvPPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesMediumPool { get; }
        public IPvPPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesLargePool { get; }
        public IPvPPool<PvPSmartMissileController, PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>> MissilesSmartPool { get; }

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

        private IPvPPool<TProjectile, TArgs> CreatePool<TProjectile, TArgs, TStats>(IPvPFactoryProvider factoryProvider, PvPProjectileKey projectileKey, int initialCapacity)
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

        public async Task SetInitialCapacity()
        {
            await BulletsPool.AddCapacity(PvPInitialCapacity.BULLET);
            await HighCalibreBulletsPool.AddCapacity(PvPInitialCapacity.BULLET);
            await TinyBulletsPool.AddCapacity(PvPInitialCapacity.BULLET);
            await ShellsSmallPool.AddCapacity(PvPInitialCapacity.SHELL_SMALL);
            await ShellsLargePool.AddCapacity(PvPInitialCapacity.SHELL_LARGE);
            await NovaShellPool.AddCapacity(PvPInitialCapacity.SHELL_LARGE);
            await RocketShellPool.AddCapacity(PvPInitialCapacity.SHELL_LARGE);
            await BombsPool.AddCapacity(PvPInitialCapacity.BOMB);
            await RocketsPool.AddCapacity(PvPInitialCapacity.ROCKET);
            await RocketsSmallPool.AddCapacity(PvPInitialCapacity.ROCKET);
            await MissilesSmallPool.AddCapacity(PvPInitialCapacity.MISSILE_SMALL);
            await MissilesMediumPool.AddCapacity(PvPInitialCapacity.MISSILE_MEDIUM);
            await MissilesLargePool.AddCapacity(PvPInitialCapacity.MISSILE_LARGE);
            await MissilesSmartPool.AddCapacity(PvPInitialCapacity.MISSILE_SMART);
        }
    }
}