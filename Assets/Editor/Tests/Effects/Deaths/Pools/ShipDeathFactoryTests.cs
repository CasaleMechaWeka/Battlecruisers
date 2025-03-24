using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Deaths;
using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Effects.Deaths.Pools
{
    public class ShipDeathFactoryTests
    {
        private IPoolableFactory<IPoolable<Vector3>, Vector3> _factory;
        private PrefabFactory _prefabFactory;
        private ShipDeathKey _shipDeathKey;
        private IPoolable<Vector3> _shipDeath;

        [SetUp]
        public void TestSetup()
        {
            _prefabFactory = Substitute.For<PrefabFactory>();
            _shipDeathKey = StaticPrefabKeys.ShipDeaths.Archon;

            _factory = new ShipDeathFactory(_prefabFactory, _shipDeathKey);

            _shipDeath = Substitute.For<IPoolable<Vector3>>();
            _prefabFactory.CreateShipDeath(_shipDeathKey).Returns(_shipDeath);
        }

        [Test]
        public void CreateItem()
        {
            IPoolable<Vector3> shipDeath = _factory.CreateItem();
            Assert.AreSame(_shipDeath, shipDeath);
        }
    }
}