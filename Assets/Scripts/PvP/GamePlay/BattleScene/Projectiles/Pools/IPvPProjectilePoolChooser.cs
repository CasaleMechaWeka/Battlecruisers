using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public interface IPvPProjectilePoolChooser<TPvPProjectile, TPvPActivationArgs, TPvPStats>
        where TPvPProjectile : PvPProjectileControllerBase<TPvPActivationArgs, TPvPStats>
        where TPvPActivationArgs : PvPProjectileActivationArgs<TPvPStats>
        where TPvPStats : IPvPProjectileStats
    {
        IPvPPool<TPvPProjectile, TPvPActivationArgs> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider);
    }
}