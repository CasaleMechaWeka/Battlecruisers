using System;
using BattleCruisers.AI;
using BattleCruisers.AI.BuildOrders;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.BuildOrders
{
    public class AntiUnitBuildOrderTests
	{
        private IDynamicBuildOrder _buildOrder;
		private IPrefabKey _basicDefenceKey, _advancedDefenceKey;
        private ILevelInfo _levelInfo;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;

			_basicDefenceKey = Substitute.For<IPrefabKey>();
            _advancedDefenceKey = Substitute.For<IPrefabKey>();
            _levelInfo = Substitute.For<ILevelInfo>();

            _buildOrder
                = new AntiUnitBuildOrder(
                    _basicDefenceKey,
                    _advancedDefenceKey,
                    _levelInfo,
                    numOfSlotsToUse: 1);
		}

		[Test]
		public void MoveNext_CanBuildAdvanced_CurrentIsAdvanced()
		{
            _levelInfo.CanConstructBuilding(_advancedDefenceKey).Returns(true);

            bool hasKey = _buildOrder.MoveNext();

            Assert.IsTrue(hasKey);
            Assert.AreSame(_advancedDefenceKey, _buildOrder.Current);
		}

        [Test]
        public void MoveNext_CannotBuildAdvanced_CanBuildBasic_CurrentIsBasic()
		{
			_levelInfo.CanConstructBuilding(_advancedDefenceKey).Returns(false);
            _levelInfo.CanConstructBuilding(_basicDefenceKey).Returns(true);
			
            bool hasKey = _buildOrder.MoveNext();
			
            Assert.IsTrue(hasKey);
            Assert.AreSame(_basicDefenceKey, _buildOrder.Current);
		}

		[Test]
		public void MoveNext_CannotBuildAdvanced_CannnotBuildBasic_Throws()
		{
			_levelInfo.CanConstructBuilding(_advancedDefenceKey).Returns(false);
            _levelInfo.CanConstructBuilding(_basicDefenceKey).Returns(false);

            Assert.Throws<ArgumentException>(() => _buildOrder.MoveNext());
		}

        [Test]
        public void MoveNext_NoMoreSlots_CurrentIsNull()
        {
            // Use up only available slot
            MoveNext_CanBuildAdvanced_CurrentIsAdvanced();

            bool hasKey = _buildOrder.MoveNext();
            Assert.IsFalse(hasKey);
            Assert.IsNull(_buildOrder.Current);
        }
	}
}
