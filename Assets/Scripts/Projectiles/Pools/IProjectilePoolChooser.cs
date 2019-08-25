using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public interface IProjectilePoolChooser<TActivationArgs, TStats>
        where TActivationArgs : ProjectileActivationArgs<TStats>
        where TStats : IProjectileStats
    {
        IPool<ProjectileControllerBase<TActivationArgs, TStats>, TActivationArgs> ChoosePool(IProjectilePoolProvider projectilePoolProvider);
    }
}