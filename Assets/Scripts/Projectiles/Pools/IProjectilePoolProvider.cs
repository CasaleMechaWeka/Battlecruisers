using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public interface IProjectilePoolProvider
    {
        IPool<ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        IPool<ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        IPool<TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
        IPool<TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        IPool<TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        IPool<TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        IPool<ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        IPool<ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
    }
}