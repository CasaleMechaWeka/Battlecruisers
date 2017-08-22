using System.Collections.Generic;
using BattleCruisers.AI;
using BattleCruisers.AI.Providers;
using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Providers
{
    public class InfiniteBuildOrderTests
	{
        private ILevelInfo _levelInfo;
		private IList<IPrefabKey> _availableBuildings;
        private BuildingCategory _buildingCategory;
        private IPrefabKey _constructableKey, _notConstructableKey;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;

            _availableBuildings = new List<IPrefabKey>();
            _buildingCategory = BuildingCategory.Ultra;
			_constructableKey = Substitute.For<IPrefabKey>();
			_notConstructableKey = Substitute.For<IPrefabKey>();

            _levelInfo = Substitute.For<ILevelInfo>();
            _levelInfo.GetAvailableBuildings(_buildingCategory).Returns(_availableBuildings);
			_levelInfo.CanConstructBuilding(_constructableKey).Returns(true);
			_levelInfo.CanConstructBuilding(_notConstructableKey).Returns(false);
		}

		[Test]
		public void Constructor_NoAvailableBuildngs_Throws()
		{
            Assert.Throws<UnityAsserts.AssertionException>(() => new InfiniteBuildOrder(_buildingCategory, _levelInfo));
		}

        [Test]
        public void MoveNext_NoConstructableBuilding_CurrentIsNull()
        {
            _availableBuildings.Add(_notConstructableKey);
            IDynamicBuildOrder buildOrder = new InfiniteBuildOrder(_buildingCategory, _levelInfo);

            bool hasKey = buildOrder.MoveNext();

            Assert.IsFalse(hasKey);
            Assert.IsNull(buildOrder.Current);
        }

		[Test]
		public void MoveNext_ConstructableBuilding_CurrentIsBuilding()
		{
            _availableBuildings.Add(_constructableKey);
			IDynamicBuildOrder buildOrder = new InfiniteBuildOrder(_buildingCategory, _levelInfo);

			bool hasKey = buildOrder.MoveNext();

            Assert.IsTrue(hasKey);
            Assert.AreSame(_constructableKey, buildOrder.Current);
		}

        [Test]
        public void MoveNext_ConstructableBuilding_And_NotConstructableBuilding_CurrentIsConstructableBuilding()
        {
            _availableBuildings.Add(_constructableKey);
            _availableBuildings.Add(_notConstructableKey);
            IDynamicBuildOrder buildOrder = new InfiniteBuildOrder(_buildingCategory, _levelInfo);

            bool hasKey = buildOrder.MoveNext();

            Assert.IsTrue(hasKey);
            Assert.AreSame(_constructableKey, buildOrder.Current);
        }
	}
}
