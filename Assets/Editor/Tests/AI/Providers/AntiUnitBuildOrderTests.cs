using System;
using BattleCruisers.AI.Providers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Providers
{
	public class AntiUnitBuildOrderTests
	{
        private IDynamicBuildOrder _buildOrder;
		private IPrefabKey _basicDefenceKey, _advancedDefenceKey;
        private IBuildableWrapper<IBuilding> _basicDefenceBuildingWrapper, _advancedDefenceBuildingWrapper;
		private IBuilding _basicDefenceBuilding, _advancedDefenceBuilding;
		private IDroneManager _droneManager;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;

			_basicDefenceKey = Substitute.For<IPrefabKey>();
            _advancedDefenceKey = Substitute.For<IPrefabKey>();
			_basicDefenceBuilding = Substitute.For<IBuilding>();
            _basicDefenceBuilding.NumOfDronesRequired.Returns(2);
            _advancedDefenceBuilding = Substitute.For<IBuilding>();
            _advancedDefenceBuilding.NumOfDronesRequired.Returns(6);

			_basicDefenceBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _basicDefenceBuildingWrapper.Buildable.Returns(_basicDefenceBuilding);
            _advancedDefenceBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _advancedDefenceBuildingWrapper.Buildable.Returns(_advancedDefenceBuilding);

            IPrefabFactory prefabFactory = Substitute.For<IPrefabFactory>();
            prefabFactory.GetBuildingWrapperPrefab(_basicDefenceKey).Returns(_basicDefenceBuildingWrapper);
            prefabFactory.GetBuildingWrapperPrefab(_advancedDefenceKey).Returns(_advancedDefenceBuildingWrapper);

            _droneManager = Substitute.For<IDroneManager>();

            _buildOrder
                = new AntiUnitBuildOrder(
                    _basicDefenceKey,
                    _advancedDefenceKey,
                    prefabFactory,
                    _droneManager,
                    numOfSlotsToUse: 1);
		}

		[Test]
		public void MoveNext_CanAffordAdvanced_CurrentIsAdvanced()
		{
            _droneManager.NumOfDrones = _advancedDefenceBuilding.NumOfDronesRequired;
            bool hasKey = _buildOrder.MoveNext();
            Assert.IsTrue(hasKey);
            Assert.AreSame(_advancedDefenceKey, _buildOrder.Current);
		}

        [Test]
        public void MoveNext_CannotAffordAdvanced_CanAffordBasic_CurrentIsBasic()
		{
            _droneManager.NumOfDrones = _basicDefenceBuilding.NumOfDronesRequired;
			bool hasKey = _buildOrder.MoveNext();
			Assert.IsTrue(hasKey);
            Assert.AreSame(_basicDefenceKey, _buildOrder.Current);
		}

		[Test]
		public void MoveNext_CannotAffordAdvanced_CannnotAffordBasic_Throws()
		{
			_droneManager.NumOfDrones = _basicDefenceBuilding.NumOfDronesRequired - 1;
            Assert.Throws<ArgumentException>(() => _buildOrder.MoveNext());
		}

        [Test]
        public void MoveNext_NoMoreSlots_CurrentIsNull()
        {
            // Use up only available slot
            MoveNext_CanAffordAdvanced_CurrentIsAdvanced();

            bool hasKey = _buildOrder.MoveNext();
            Assert.IsFalse(hasKey);
            Assert.IsNull(_buildOrder.Current);
        }
	}
}
