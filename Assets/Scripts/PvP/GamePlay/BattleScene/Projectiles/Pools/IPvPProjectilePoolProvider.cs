using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public interface IPvPProjectilePoolProvider
    {
        Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>> BulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>> HighCalibreBulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>> TinyBulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>> FlakBulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>> ShellsLargePool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>> NovaShellPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>> FiveShellCluster { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>> RocketShellPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>> ShellsSmallPool { get; }
        Pool<PvPBombController, ProjectileActivationArgs<ProjectileStats>> BombsPool { get; }
        Pool<PvPBombController, ProjectileActivationArgs<ProjectileStats>> StratBombsPool { get; }
        Pool<PvPRocketController, TargetProviderActivationArgs<CruisingProjectileStats>> RocketsPool { get; }
        Pool<PvPMissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesSmallPool { get; }
        Pool<PvPRocketController, TargetProviderActivationArgs<CruisingProjectileStats>> RocketsSmallPool { get; }
        Pool<PvPMissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesMediumPool { get; }
        Pool<PvPMissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesMFPool { get; }
        Pool<PvPMissileController, TargetProviderActivationArgs<ProjectileStats>> RailSlugsPool { get; }
        Pool<PvPRocketController, TargetProviderActivationArgs<CruisingProjectileStats>> MissilesFirecrackerPool { get; }
        Pool<PvPMissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesLargePool { get; }
        Pool<PvPSmartMissileController, PvPSmartMissileActivationArgs<SmartProjectileStats>> MissilesSmartPool { get; }

    }
}