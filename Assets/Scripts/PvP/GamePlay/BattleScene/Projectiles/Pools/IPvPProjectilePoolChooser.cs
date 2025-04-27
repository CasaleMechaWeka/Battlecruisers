using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public interface IPvPProjectilePoolChooser<TPvPProjectile, TPvPActivationArgs, TPvPStats>
        where TPvPProjectile : PvPProjectileControllerBase<TPvPActivationArgs, TPvPStats>
        where TPvPActivationArgs : ProjectileActivationArgs<TPvPStats>
        where TPvPStats : ProjectileStats
    {
        Pool<TPvPProjectile, TPvPActivationArgs> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider);
    }
}