using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class ExplosionFactory : IPoolableFactory<IPoolable<Vector3>, Vector3>
    {
        private readonly ExplosionKey _explosionKey;

        public ExplosionFactory(ExplosionKey explosionKey)
        {
            Helper.AssertIsNotNull(explosionKey);

            _explosionKey = explosionKey;
        }

        public IPoolable<Vector3> CreateItem()
        {
            return PrefabFactory.CreateExplosion(_explosionKey);
        }

        public override string ToString()
        {
            return $"{nameof(ExplosionFactory)} {_explosionKey}";
        }
    }
}