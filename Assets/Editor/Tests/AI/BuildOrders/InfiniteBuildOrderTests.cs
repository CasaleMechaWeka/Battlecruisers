using BattleCruisers.AI;
using BattleCruisers.AI.BuildOrders;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.BuildOrders
{
    public class InfiniteBuildOrderTests
	{
        private ILevelInfo _levelInfo;
        private IList<BuildingKey> _availableBuildings, _bannedBuildings;
        private BuildingCategory _buildingCategory;
        private BuildingKey _constructableKey, _notConstructableKey;

		[SetUp]
		public void SetuUp()
		{
            _availableBuildings = new List<BuildingKey>();
            _bannedBuildings = new List<BuildingKey>();
            _buildingCategory = BuildingCategory.Ultra;
            _constructableKey = new BuildingKey(BuildingCategory.Defence, "Park");
            _notConstructableKey = new BuildingKey(BuildingCategory.Offence, "City");

            _levelInfo = Substitute.For<ILevelInfo>();
            _levelInfo.GetAvailableBuildings(_buildingCategory).Returns(_availableBuildings);
			_levelInfo.CanConstructBuilding(_constructableKey).Returns(true);
			_levelInfo.CanConstructBuilding(_notConstructableKey).Returns(false);
		}

		[Test]
		public void Constructor_NoAvailableBuildngs_Throws()
		{
            Assert.Throws<UnityAsserts.AssertionException>(() => new InfiniteBuildOrder(_buildingCategory, _levelInfo, _bannedBuildings));
		}

        [Test]
        public void Constructor_NoAvailableNonBannedBuildngs_Throws()
        {
            _availableBuildings.Add(_constructableKey);
            _bannedBuildings.Add(_constructableKey);

            Assert.Throws<UnityAsserts.AssertionException>(() => new InfiniteBuildOrder(_buildingCategory, _levelInfo, _bannedBuildings));
        }

        [Test]
        public void MoveNext_NoConstructableBuilding_CurrentIsNull()
        {
            _availableBuildings.Add(_notConstructableKey);
            IDynamicBuildOrder buildOrder = new InfiniteBuildOrder(_buildingCategory, _levelInfo, _bannedBuildings);

            bool hasKey = buildOrder.MoveNext();

            Assert.IsFalse(hasKey);
            Assert.IsNull(buildOrder.Current);
        }

		[Test]
		public void MoveNext_ConstructableBuilding_CurrentIsBuilding()
		{
            _availableBuildings.Add(_constructableKey);
            IDynamicBuildOrder buildOrder = new InfiniteBuildOrder(_buildingCategory, _levelInfo, _bannedBuildings);

			bool hasKey = buildOrder.MoveNext();

            Assert.IsTrue(hasKey);
            Assert.AreSame(_constructableKey, buildOrder.Current);
		}

        [Test]
        public void MoveNext_ConstructableBuilding_And_NotConstructableBuilding_CurrentIsConstructableBuilding()
        {
            _availableBuildings.Add(_constructableKey);
            _availableBuildings.Add(_notConstructableKey);
            IDynamicBuildOrder buildOrder = new InfiniteBuildOrder(_buildingCategory, _levelInfo, _bannedBuildings);

            bool hasKey = buildOrder.MoveNext();

            Assert.IsTrue(hasKey);
            Assert.AreSame(_constructableKey, buildOrder.Current);
        }
	}
}
