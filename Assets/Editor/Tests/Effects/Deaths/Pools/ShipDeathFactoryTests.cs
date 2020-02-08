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
        private IPoolableFactory<IShipDeath, Vector3> _factory;
        private IPrefabFactory _prefabFactory;
        private ShipDeathKey _shipDeathKey;
        private IShipDeath _shipDeath;

        [SetUp]
        public void TestSetup()
        {
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _shipDeathKey = StaticPrefabKeys.ShipDeaths.Archon;

            _factory = new ShipDeathFactory(_prefabFactory, _shipDeathKey);

            _shipDeath = Substitute.For<IShipDeath>();
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