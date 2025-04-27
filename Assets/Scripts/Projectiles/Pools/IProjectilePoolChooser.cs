using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public interface IProjectilePoolChooser<TProjectile, TActivationArgs, TStats>
        where TProjectile : ProjectileControllerBase<TActivationArgs, TStats>
        where TActivationArgs : ProjectileActivationArgs<TStats>
        where TStats : ProjectileStats
    {
        Pool<TProjectile, TActivationArgs> ChoosePool(IProjectilePoolProvider projectilePoolProvider);
    }
}