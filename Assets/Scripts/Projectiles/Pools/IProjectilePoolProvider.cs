using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public interface IProjectilePoolProvider
    {
        IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        IPool<BombController, ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        IPool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
    }
}