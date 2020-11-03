using BattleCruisers.AI.BuildOrders;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.AI.BuildOrders
{
    public class CombinedBuildOrdersTests
	{
        private IDynamicBuildOrder _comboBuildOrder, _buildOrder1, _buildOrder2;
        private BuildingKey _key1, _key2;

		[SetUp]
		public void SetuUp()
		{
            _key1 = new BuildingKey(BuildingCategory.Tactical, "Raeuber");
            _buildOrder1 = CreateBuildOrder(_key1);

            _key2 = new BuildingKey(BuildingCategory.Tactical, "Hotzenplotz");
            _buildOrder2 = CreateBuildOrder(_key2);

            _comboBuildOrder = new CombinedBuildOrders(new List<IDynamicBuildOrder>() { _buildOrder1, _buildOrder2 });
        }

        private IDynamicBuildOrder CreateBuildOrder(IPrefabKey key)
        {
            IDynamicBuildOrder buildOrder = Substitute.For<IDynamicBuildOrder>();
			buildOrder.Current.Returns(key, null, null, null);
			buildOrder.MoveNext().Returns(true, false, false, false);
            return buildOrder;
        }

		[Test]
		public void MoveNext()
		{
            Assert.IsTrue(_comboBuildOrder.MoveNext());
            Assert.AreSame(_key1, _comboBuildOrder.Current);

			Assert.IsTrue(_comboBuildOrder.MoveNext());
			Assert.AreSame(_key2, _comboBuildOrder.Current);

			Assert.IsFalse(_comboBuildOrder.MoveNext());
            Assert.IsNull(_comboBuildOrder.Current);
		}
	}
}
