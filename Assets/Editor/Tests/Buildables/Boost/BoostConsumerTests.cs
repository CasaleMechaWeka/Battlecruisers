using BattleCruisers.Buildables.Boost;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Boost
{
    public class BoostConsumerTests
	{
        private IBoostConsumer _boostConsumer;
        private IBoostProvider _provider1, _provider2;
        private int _eventCount;

		[SetUp]
		public void SetuUp()
		{
            _eventCount = 0;

            _boostConsumer = new BoostConsumer();
            _boostConsumer.BoostChanged += (sender, e) => _eventCount++;

            _provider1 = Substitute.For<IBoostProvider>();
            _provider1.BoostMultiplier.Returns(1.2f);

            _provider2 = Substitute.For<IBoostProvider>();
            _provider2.BoostMultiplier.Returns(1.5f);
		}

		[Test]
		public void InitialBoost()
		{
            Assert.AreEqual(1, _boostConsumer.CumulativeBoost);
		}

        [Test]
        public void AddProvider_UpdatesBoost_EmitsChangedEvent()
        {
            _boostConsumer.AddBoostProvider(_provider1);

            Assert.AreEqual(_provider1.BoostMultiplier, _boostConsumer.CumulativeBoost);
            Assert.AreEqual(1, _eventCount);
        }

        [Test]
        public void AddMultipleProviders_UpdatesBoost_EmitsChangedEvent()
        {
			_boostConsumer.AddBoostProvider(_provider1);

			Assert.AreEqual(_provider1.BoostMultiplier, _boostConsumer.CumulativeBoost);
			Assert.AreEqual(1, _eventCount);

			_boostConsumer.AddBoostProvider(_provider2);

            Assert.AreEqual(_provider1.BoostMultiplier * _provider2.BoostMultiplier, _boostConsumer.CumulativeBoost);
			Assert.AreEqual(2, _eventCount);
        }

        [Test]
        public void AddDuplicateProvider_Throws()
        {
			_boostConsumer.AddBoostProvider(_provider1);
            Assert.Throws<UnityAsserts.AssertionException>(() => _boostConsumer.AddBoostProvider(_provider1));
        }

        [Test]
        public void RemoveProvider_UpdatesBoost_EmitsChangedEvent()
        {
			_boostConsumer.AddBoostProvider(_provider1);

			Assert.AreEqual(_provider1.BoostMultiplier, _boostConsumer.CumulativeBoost);
			Assert.AreEqual(1, _eventCount);

            _boostConsumer.RemoveBoostProvider(_provider1);

            Assert.AreEqual(1, _boostConsumer.CumulativeBoost);
            Assert.AreEqual(2, _eventCount);
        }

        [Test]
        public void RemoveNotAddedProvider_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _boostConsumer.RemoveBoostProvider(_provider1));
        }
	}
}
