using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPProjectileFactory<TProjectile, TActivationArgs, TStats> : IPoolableFactory<TProjectile, TActivationArgs>
        where TActivationArgs : ProjectileActivationArgs
        where TProjectile : PvPProjectileControllerBase<TActivationArgs, TStats>
        where TStats : ProjectileStats
    {
        private readonly PvPProjectileKey _projectileKey;

        public PvPProjectileFactory(PvPProjectileKey projectileKey)
        {
            PvPHelper.AssertIsNotNull(projectileKey);

            _projectileKey = projectileKey;
        }

        public TProjectile CreateItem()
        {
            return PvPPrefabFactory.CreateProjectile<TProjectile, TActivationArgs, TStats>(_projectileKey);
        }

        public override string ToString()
        {
            return $"{nameof(PvPProjectileFactory<TProjectile, TActivationArgs, TStats>)} {_projectileKey}";
        }
    }
}