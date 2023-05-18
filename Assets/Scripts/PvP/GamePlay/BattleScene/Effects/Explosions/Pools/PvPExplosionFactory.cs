using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public class PvPExplosionFactory : IPvPPoolableFactory<IPvPExplosion, Vector3>
    {
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly PvPExplosionKey _explosionKey;

        public PvPExplosionFactory(IPvPPrefabFactory prefabFactory, PvPExplosionKey explosionKey)
        {
            PvPHelper.AssertIsNotNull(prefabFactory, explosionKey);

            _prefabFactory = prefabFactory;
            _explosionKey = explosionKey;
        }

        public IPvPExplosion CreateItem()
        {
            return _prefabFactory.CreateExplosion(_explosionKey);
        }

        public override string ToString()
        {
            return $"{nameof(PvPExplosionFactory)} {_explosionKey}";
        }
    }
}