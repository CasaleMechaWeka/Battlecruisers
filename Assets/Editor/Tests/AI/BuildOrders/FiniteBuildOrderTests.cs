using BattleCruisers.AI.BuildOrders;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.BuildOrders
{
    public class FiniteBuildOrderTests
	{
		private IDynamicBuildOrder _finiteBuildOrder, _infiniteBuildOrder;
        private BuildingKey _key;

        [SetUp]
		public void SetuUp()
		{
            _key = new BuildingKey(BuildingCategory.Tactical, "Zauberer");
            _infiniteBuildOrder = Substitute.For<IDynamicBuildOrder>();
            _infiniteBuildOrder.Current.Returns(_key);
            _infiniteBuildOrder.MoveNext().Returns(true);

            _finiteBuildOrder = new FiniteBuildOrder(_infiniteBuildOrder, size: 1);
		}

		[Test]
		public void MoveNext_WithinSize_ReturnsTrue()
		{
            bool hasKey = _finiteBuildOrder.MoveNext();

            Assert.IsTrue(hasKey);
            Assert.AreSame(_key, _finiteBuildOrder.Current);
		}

        [Test]
        public void MoveNext_OutsideOfSize_ReturnsFalse()
		{
            MoveNext_WithinSize_ReturnsTrue();

			bool hasKey = _finiteBuildOrder.MoveNext();

            Assert.IsFalse(hasKey);
            Assert.IsNull(_finiteBuildOrder.Current);
		}

        [Test]
        public void MoveNext_InfiniteBuildOrder_ReturnsFalse_Throws()
        {
            _infiniteBuildOrder.MoveNext().Returns(false);

            Assert.Throws<UnityAsserts.AssertionException>(() => _finiteBuildOrder.MoveNext());
        }
	}
}
