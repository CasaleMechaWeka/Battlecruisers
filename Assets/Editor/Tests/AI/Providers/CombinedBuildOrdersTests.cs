using System.Collections.Generic;
using BattleCruisers.AI.Providers;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Providers
{
    public class CombinedBuildOrdersTests
	{
        private IDynamicBuildOrder _comboBuildOrder, _buildOrder1, _buildOrder2;
        private IPrefabKey _key1, _key2;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;

            _key1 = Substitute.For<IPrefabKey>();
            _buildOrder1 = CreateBuildOrder(_key1);

            _key2 = Substitute.For<IPrefabKey>();
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
