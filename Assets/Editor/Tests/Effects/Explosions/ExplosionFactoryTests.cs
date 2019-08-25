using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Effects.Explosions
{
    public class ExplosionFactoryTests
    {
        private IPoolableFactory<IExplosion, Vector3> _factory;
        private IPrefabFactory _prefabFactory;
        private ExplosionKey _explosionKey;
        private IExplosion _explosion;

        [SetUp]
        public void TestSetup()
        {
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _explosionKey = StaticPrefabKeys.Explosions.HDExplosion150;

            _factory = new ExplosionFactory(_prefabFactory, _explosionKey);

            _explosion = Substitute.For<IExplosion>();
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