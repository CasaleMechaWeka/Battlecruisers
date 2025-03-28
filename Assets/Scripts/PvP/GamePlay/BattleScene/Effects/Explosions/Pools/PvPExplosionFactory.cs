using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public class PvPExplosionFactory : IPoolableFactory<IPoolable<Vector3>, Vector3>
    {
        private readonly PvPPrefabFactory _prefabFactory;
        private readonly PvPExplosionKey _explosionKey;

        public PvPExplosionFactory(PvPPrefabFactory prefabFactory, PvPExplosionKey explosionKey)
        {
            PvPHelper.AssertIsNotNull(prefabFactory, explosionKey);

            _prefabFactory = prefabFactory;
            _explosionKey = explosionKey;
        }

        public IPoolable<Vector3> CreateItem()
        {
            return _prefabFactory.CreateExplosion(_explosionKey);
        }

        public override string ToString()
        {
            return $"{nameof(PvPExplosionFactory)} {_explosionKey}";
        }
    }
}