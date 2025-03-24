using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Effects.Explosions
{
    public class ExplosionFactoryTests
    {
        private IPoolableFactory<IPoolable<Vector3>, Vector3> _factory;
        private PrefabFactory _prefabFactory;
        private ExplosionKey _explosionKey;
        private IPoolable<Vector3> _explosion;

        [SetUp]
        public void TestSetup()
        {
            _prefabFactory = Substitute.For<PrefabFactory>();
            _explosionKey = StaticPrefabKeys.Explosions.Explosion150;

            _factory = new ExplosionFactory(_prefabFactory, _explosionKey);

            _explosion = Substitute.For<IPoolable<Vector3>>();
            _prefabFactory.CreateExplosion(_explosionKey).Returns(_explosion);
        }

        [Test]
        public void CreateItem()
        {
            IPoolable<Vector3> explosion = _factory.CreateItem();
            Assert.AreSame(_explosion, explosion);
        }
    }
}