using System.Collections.Generic;
using BattleCruisers.AI;
using BattleCruisers.AI.BuildOrders;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.BuildOrders
{
    public class StrategyBuildOrderTests
    {
        private IDynamicBuildOrder _buildOrder;

        private IEnumerator<IPrefabKeyWrapper> _baseBuildOrder;
        private IPrefabKeyWrapper _keyWrapper;
        private BuildingKey _buildingKey;
        private ILevelInfo _levelInfo;

        [SetUp]
        public void SetuUp()
        {
            _keyWrapper = Substitute.For<IPrefabKeyWrapper>();
            _buildingKey = new BuildingKey(BuildingCategory.Defence, "soul 7");
            _keyWrapper.Key.Returns(_buildingKey);

            _baseBuildOrder = Substitute.For<IEnumerator<IPrefabKeyWrapper>>();
            _baseBuildOrder.Current.Returns(_keyWrapper);

            _levelInfo = Substitute.For<ILevelInfo>();

            _buildOrder = new StrategyBuildOrder(_baseBuildOrder, _levelInfo);
        }

        [Test]
        public void Current_InitiallyNull()
        {
            Assert.IsNull(_buildOrder.Current);
        }

        [Test]
        public void EmptyBuildOrder()
        {
            _baseBuildOrder.MoveNext().Returns(false);

            Assert.IsFalse(_buildOrder.MoveNext());
            Assert.IsNull(_buildOrder.Current);
        }

        [Test]
        public void NonEmptyBuildOrder_PrefabKeyWrapper_HasNoKey()
        {
            // Pretend to only have one key in build order, so only return true once
            _baseBuildOrder.MoveNext().Returns(true, false);

            _keyWrapper.HasKey.Returns(false);

            Assert.IsFalse(_buildOrder.MoveNext());
            Assert.IsNull(_buildOrder.Current);
        }

        [Test]
        public void NonEmptyBuildOrder_PrefabKeyWrapper_HasKey_CannotConstructBuilding()
        {
            // Pretend to only have one key in build order, so only return true once
            _baseBuildOrder.MoveNext().Returns(true, false);

            _keyWrapper.HasKey.Returns(true);
            _levelInfo.CanConstructBuilding(_buildingKey).Returns(false);

            Assert.IsFalse(_buildOrder.MoveNext());
            Assert.IsNull(_buildOrder.Current);

            _levelInfo.Received().CanConstructBuilding(_buildingKey);
        }

        [Test]
        public void NonEmptyBuildOrder_PrefabKeyWrapper_HasKey_CanConstructBuilding()
        {
            // Pretend to only have one key in build order, so only return true once
            _baseBuildOrder.MoveNext().Returns(true, false);

            _keyWrapper.HasKey.Returns(true);
            _levelInfo.CanConstructBuilding(_buildingKey).Returns(true);

            Assert.IsTrue(_buildOrder.MoveNext());
            Assert.AreSame(_buildingKey, _buildOrder.Current);

            _levelInfo.Received().CanConstructBuilding(_buildingKey);
        }

        [Test]
        public void FirstKeyNotConstructable_SecondKeyIsConstructable()
        {
            // Pretend to only have 2 keys in build order, so only return true twice
            _baseBuildOrder.MoveNext().Returns(true, true, false);

            // First key is not constructable, second key is
			_keyWrapper.HasKey.Returns(true, true);
            _levelInfo.CanConstructBuilding(_buildingKey).Returns(false, true);

            Assert.IsTrue(_buildOrder.MoveNext());
            Assert.AreSame(_buildingKey, _buildOrder.Current);

            _levelInfo.Received(2).CanConstructBuilding(_buildingKey);
        }
    }
}
