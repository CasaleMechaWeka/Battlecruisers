using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions.Pools
{
    // FELIX  Remove :)
    public class BulletImpactExplosionFactory : IPoolableFactory<IExplosion, Vector3>
    {
        private readonly IPrefabFactory _prefabFactory;

        public BulletImpactExplosionFactory(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        public IExplosion CreateItem()
        {
            return _prefabFactory.CreateBulletImpactExplosion();
        }
    }
}