using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public class PvPExplosionFactory : IPoolableFactory<IPoolable<Vector3>, Vector3>
    {
        private readonly PvPExplosionKey _explosionKey;

        public PvPExplosionFactory(PvPExplosionKey explosionKey)
        {
            PvPHelper.AssertIsNotNull(explosionKey);

            _explosionKey = explosionKey;
        }

        public IPoolable<Vector3> CreateItem()
        {
            return PvPPrefabFactory.CreateExplosion(_explosionKey);
        }

        public override string ToString()
        {
            return $"{nameof(PvPExplosionFactory)} {_explosionKey}";
        }
    }
}