using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public interface IProjectilePoolChooser<TProjectile, TActivationArgs>
        where TProjectile : ProjectileControllerBase<TActivationArgs>
        where TActivationArgs : ProjectileActivationArgs
    {
        Pool<TProjectile, TActivationArgs> ChoosePool(IProjectilePoolProvider projectilePoolProvider);
    }
}