using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public interface IPvPProjectilePoolProvider
    {
        Pool<PvPProjectileController, ProjectileActivationArgs> BulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs> HighCalibreBulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs> TinyBulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs> FlakBulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs> ShellsLargePool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs> NovaShellPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs> FiveShellCluster { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs> RocketShellPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs> ShellsSmallPool { get; }
        Pool<PvPBombController, ProjectileActivationArgs> BombsPool { get; }
        Pool<PvPBombController, ProjectileActivationArgs> StratBombsPool { get; }
        Pool<PvPRocketController, ProjectileActivationArgs> RocketsPool { get; }
        Pool<PvPMissileController, ProjectileActivationArgs> MissilesSmallPool { get; }
        Pool<PvPRocketController, ProjectileActivationArgs> RocketsSmallPool { get; }
        Pool<PvPMissileController, ProjectileActivationArgs> MissilesMediumPool { get; }
        Pool<PvPMissileController, ProjectileActivationArgs> MissilesMFPool { get; }
        Pool<PvPMissileController, ProjectileActivationArgs> RailSlugsPool { get; }
        Pool<PvPRocketController, ProjectileActivationArgs> MissilesFirecrackerPool { get; }
        Pool<PvPMissileController, ProjectileActivationArgs> MissilesLargePool { get; }
        Pool<PvPSmartMissileController, PvPProjectileActivationArgs> MissilesSmartPool { get; }

    }
}