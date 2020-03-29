using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles
{
    /// <summary>
    /// Projectiles with trails (eg: rockets) should not have the trail disappear on impact.
    /// Instead, the projectile should be inert, but set the trail hang around and dissipate
    /// before deactivating completely and being recycled.
    /// </summary>
    /// FELIX  Create bomb controller?  So that trail also gets handled correctly :)
    public abstract class ProjectileWithTrail<TActivationArgs, TStats> : ProjectileControllerBase<TActivationArgs, TStats>,
        IRemovable,
        IPoolable<TActivationArgs>
            where TActivationArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
    {
        protected override void DestroyProjectile()
        {
            ShowExplosion();
            RemoveFromScene();
        }
    }
}