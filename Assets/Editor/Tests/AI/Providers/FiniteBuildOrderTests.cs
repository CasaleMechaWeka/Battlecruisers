using BattleCruisers.AI.Providers;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Providers
{
    public class FiniteBuildOrderTests
	{
		private IDynamicBuildOrder _finiteBuildOrder, _infiniteBuildOrder;
        private IPrefabKey _key;

        [SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;

            _key = Substitute.For<IPrefabKey>();
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
