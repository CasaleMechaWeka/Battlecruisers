using BattleCruisers.Buildables.Boost;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Boost
{
    public class BoostProviderTests
	{
        private IBoostProvider _provider;
        private IBoostConsumer _consumer1;

		[SetUp]
		public void SetuUp()
		{
            _provider = new BoostProvider(boostMultiplier: 1.8f);

            _consumer1 = Substitute.For<IBoostConsumer>();
		}

		[Test]
		public void BoostMatchesConstructoParameter()
		{
            Assert.AreEqual(1.8f, _provider.BoostMultiplier);
		}

        [Test]
        public void AddConsumer_SetsSelfAsProviderOnConsumer()
        {
            _provider.AddBoostConsumer(_consumer1);
            _consumer1.Received().AddBoostProvider(_provider);
        }

        [Test]
        public void RemoveConsumer_RemovesSelfFromConsumer()
        {
			_provider.AddBoostConsumer(_consumer1);

            _provider.RemoveBoostConsumer(_consumer1);
            _consumer1.Received().RemoveBoostProvider(_provider);
        }
	}
}
