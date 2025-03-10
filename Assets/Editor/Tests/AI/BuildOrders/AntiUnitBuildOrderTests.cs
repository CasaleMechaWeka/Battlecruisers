using BattleCruisers.AI;
using BattleCruisers.AI.BuildOrders;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.BuildOrders
{
    public class AntiUnitBuildOrderTests
	{
        private IDynamicBuildOrder _buildOrder;
        private BuildingKey _basicDefenceKey, _advancedDefenceKey;
        private ILevelInfo _levelInfo;
        private int _numOfSlotsToUse;

		[SetUp]
		public void SetuUp()
		{
            _basicDefenceKey = new BuildingKey(BuildingCategory.Defence, "Kasper");
            _advancedDefenceKey = new BuildingKey(BuildingCategory.Defence, "Seppel");
            _levelInfo = Substitute.For<ILevelInfo>();
            _numOfSlotsToUse = 2;

            _buildOrder
                = new AntiUnitBuildOrder(
                    _basicDefenceKey,
                    _advancedDefenceKey,
                    _levelInfo,
                    _numOfSlotsToUse);

            _levelInfo.CanConstructBuilding(_basicDefenceKey).Returns(true);
        }

        [Test]
        public void MoveNext_CannnotBuildBasic_Throws()
        {
            _levelInfo.CanConstructBuilding(_basicDefenceKey).Returns(false);
            Assert.Throws<UnityAsserts.AssertionException>(() => _buildOrder.MoveNext());
        }

        [Test]
        public void MoveNext_FirstKey_CurrentIsBasic()
        {
            _levelInfo.CanConstructBuilding(_advancedDefenceKey).Returns(true);

            bool hasKey = _buildOrder.MoveNext();

            Assert.IsTrue(hasKey);
            Assert.AreSame(_basicDefenceKey, _buildOrder.Current);
        }

        [Test]
		public void MoveNext_NotFirst_CanBuildAdvanced_CurrentIsAdvanced()
		{
            MoveNext_FirstKey_CurrentIsBasic();

            _levelInfo.CanConstructBuilding(_advancedDefenceKey).Returns(true);

            bool hasKey = _buildOrder.MoveNext();

            Assert.IsTrue(hasKey);
            Assert.AreSame(_advancedDefenceKey, _buildOrder.Current);
		}

        [Test]
        public void MoveNext_CannotBuildAdvanced_CurrentIsBasic()
		{
            MoveNext_FirstKey_CurrentIsBasic();

            _levelInfo.CanConstructBuilding(_advancedDefenceKey).Returns(false);
			
            bool hasKey = _buildOrder.MoveNext();
			
            Assert.IsTrue(hasKey);
            Assert.AreSame(_basicDefenceKey, _buildOrder.Current);
		}

        [Test]
        public void MoveNext_NoMoreSlots_CurrentIsNull()
        {
            // Use up only available slots
            for (int i = 0; i < _numOfSlotsToUse; ++i)
            {
                _buildOrder.MoveNext();
            }

            bool hasKey = _buildOrder.MoveNext();
            Assert.IsFalse(hasKey);
            Assert.IsNull(_buildOrder.Current);
        }
	}
}
