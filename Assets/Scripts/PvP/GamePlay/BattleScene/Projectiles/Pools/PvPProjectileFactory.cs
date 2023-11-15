using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPProjectileFactory<TProjectile, TActivationArgs, TStats> : IPvPPoolableFactory<TProjectile, TActivationArgs>
        where TActivationArgs : PvPProjectileActivationArgs<TStats>
        where TProjectile : PvPProjectileControllerBase<TActivationArgs, TStats>
        where TStats : IPvPProjectileStats
    {
        private readonly IPvPFactoryProvider _factoryProvider;
        private readonly PvPProjectileKey _projectileKey;

        public PvPProjectileFactory(IPvPFactoryProvider factoryProvider, PvPProjectileKey projectileKey)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, projectileKey);

            _factoryProvider = factoryProvider;
            _projectileKey = projectileKey;
        }

        public async Task<TProjectile> CreateItem()
        {
            return await _factoryProvider.PrefabFactory.CreateProjectile<TProjectile, TActivationArgs, TStats>(_projectileKey, _factoryProvider);
        }

        public override string ToString()
        {
            return $"{nameof(PvPProjectileFactory<TProjectile, TActivationArgs, TStats>)} {_projectileKey}";
        }
    }
}