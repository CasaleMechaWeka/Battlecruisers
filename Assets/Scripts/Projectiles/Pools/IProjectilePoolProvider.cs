using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public interface IProjectilePoolProvider
    {
        // FELIX Create concrete classes to avoid this generic mess?
        IPool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        IPool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        IPool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        IPool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        IPool<ProjectileControllerBase<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        IPool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        IPool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        IPool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
    }
}