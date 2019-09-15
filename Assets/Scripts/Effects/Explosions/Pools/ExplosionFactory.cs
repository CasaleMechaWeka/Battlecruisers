using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class ExplosionFactory : IPoolableFactory<IExplosion, Vector3>
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly ExplosionKey _explosionKey;

        public ExplosionFactory(IPrefabFactory prefabFactory, ExplosionKey explosionKey)
        {
            Helper.AssertIsNotNull(prefabFactory, explosionKey);

            _prefabFactory = prefabFactory;
            _explosionKey = explosionKey;
        }

        public IExplosion CreateItem()
        {
            return _prefabFactory.CreateExplosion(_explosionKey);
        }

        public override string ToString()
        {
            return $"{nameof(ExplosionFactory)} {_explosionKey}";
        }
    }
}