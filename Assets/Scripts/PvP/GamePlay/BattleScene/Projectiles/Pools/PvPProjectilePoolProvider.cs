using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPProjectilePoolProvider : IPvPProjectilePoolProvider
    {
        public Pool<PvPProjectileController, ProjectileActivationArgs> BulletsPool { get; }
        public Pool<PvPProjectileController, ProjectileActivationArgs> HighCalibreBulletsPool { get; }
        public Pool<PvPProjectileController, ProjectileActivationArgs> TinyBulletsPool { get; }
        public Pool<PvPProjectileController, ProjectileActivationArgs> FlakBulletsPool { get; }
        public Pool<PvPProjectileController, ProjectileActivationArgs> ShellsLargePool { get; }
        public Pool<PvPProjectileController, ProjectileActivationArgs> NovaShellPool { get; }
        public Pool<PvPProjectileController, ProjectileActivationArgs> FiveShellCluster { get; }
        public Pool<PvPProjectileController, ProjectileActivationArgs> RocketShellPool { get; }
        public Pool<PvPProjectileController, ProjectileActivationArgs> ShellsSmallPool { get; }
        public Pool<PvPBombController, ProjectileActivationArgs> BombsPool { get; }
        public Pool<PvPBombController, ProjectileActivationArgs> StratBombsPool { get; }
        public Pool<PvPRocketController, ProjectileActivationArgs> RocketsPool { get; }
        public Pool<PvPMissileController, ProjectileActivationArgs> MissilesSmallPool { get; }
        public Pool<PvPRocketController, ProjectileActivationArgs> RocketsSmallPool { get; }
        public Pool<PvPRocketController, ProjectileActivationArgs> MissilesFirecrackerPool { get; }
        public Pool<PvPMissileController, ProjectileActivationArgs> MissilesMediumPool { get; }
        public Pool<PvPMissileController, ProjectileActivationArgs> MissilesMFPool { get; }
        public Pool<PvPMissileController, ProjectileActivationArgs> RailSlugsPool { get; }
        public Pool<PvPMissileController, ProjectileActivationArgs> MissilesLargePool { get; }
        public Pool<PvPSmartMissileController, PvPProjectileActivationArgs> MissilesSmartPool { get; }

        public PvPProjectilePoolProvider()
        {
            BulletsPool
                = CreatePool<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPBullet,
                    PvPInitialCapacity.BULLET);

            HighCalibreBulletsPool
                = CreatePool<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPHighCalibreBullet,
                    PvPInitialCapacity.BULLET);

            TinyBulletsPool
                = CreatePool<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPTinyBullet,
                    PvPInitialCapacity.BULLET);

            FlakBulletsPool
                = CreatePool<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPFlakBullet,
                    PvPInitialCapacity.BULLET);

            ShellsSmallPool
                = CreatePool<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPShellSmall,
                    PvPInitialCapacity.SHELL_SMALL);

            ShellsLargePool
                = CreatePool<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPShellLarge,
                    PvPInitialCapacity.SHELL_LARGE);

            NovaShellPool
                = CreatePool<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPNovaShell,
                    PvPInitialCapacity.SHELL_LARGE);

            FiveShellCluster
                = CreatePool<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPFiveShellCluster,
                    PvPInitialCapacity.SHELL_LARGE);

            RocketShellPool
                = CreatePool<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRocketShell,
                    PvPInitialCapacity.SHELL_LARGE);


            BombsPool
                = CreatePool<PvPBombController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPBomb,
                    PvPInitialCapacity.BOMB);


            StratBombsPool
                = CreatePool<PvPBombController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPStratBomb,
                    PvPInitialCapacity.BOMB);

            RocketsPool
                = CreatePool<PvPRocketController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRocket,
                    PvPInitialCapacity.ROCKET);

            RocketsSmallPool
                = CreatePool<PvPRocketController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRocketSmall,
                    PvPInitialCapacity.ROCKET);

            MissilesSmallPool
                = CreatePool<PvPMissileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileSmall,
                    PvPInitialCapacity.MISSILE_SMALL);

            MissilesMediumPool
                = CreatePool<PvPMissileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileMedium,
                    PvPInitialCapacity.MISSILE_MEDIUM);

            MissilesMFPool
                = CreatePool<PvPMissileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileMF,
                    PvPInitialCapacity.MISSILE_MEDIUM);

            /*RailSlugsPool
                = CreatePool<PvPMissileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    factoryProvider,
                    PvPStaticPrefabKeys.PvPProjectiles.PvPRailSlug,
                    PvPInitialCapacity.MISSILE_MEDIUM);*/

            MissilesFirecrackerPool
                = CreatePool<PvPRocketController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileFirecracker,
                    PvPInitialCapacity.MISSILE_MEDIUM);

            MissilesLargePool
                = CreatePool<PvPMissileController, ProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileLarge,
                    PvPInitialCapacity.MISSILE_LARGE);

            MissilesSmartPool
                = CreatePool<PvPSmartMissileController, PvPProjectileActivationArgs, ProjectileStats>(
                    PvPStaticPrefabKeys.PvPProjectiles.PvPMissileSmart,
                    PvPInitialCapacity.MISSILE_SMART);


        }

        private Pool<TProjectile, TArgs> CreatePool<TProjectile, TArgs, TStats>(PvPProjectileKey projectileKey, int initialCapacity)
            where TArgs : ProjectileActivationArgs
            where TProjectile : PvPProjectileControllerBase<TArgs, TStats>
            where TStats : ProjectileStats
        {
            return
                new Pool<TProjectile, TArgs>(
                    new PvPProjectileFactory<TProjectile, TArgs, TStats>(projectileKey));
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
            //RailSlugsPool.AddCapacity(1);
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
            //RailSlugsPool.AddCapacity(PvPInitialCapacity.MISSILE_MEDIUM - 1);
            MissilesFirecrackerPool.AddCapacity(PvPInitialCapacity.MISSILE_MEDIUM - 1);
            MissilesLargePool.AddCapacity(PvPInitialCapacity.MISSILE_LARGE - 1);
            MissilesSmartPool.AddCapacity(PvPInitialCapacity.MISSILE_SMART - 1);
        }
    }
}