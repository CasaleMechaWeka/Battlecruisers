using BattleCruisers.Buildables.Boost;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Boost
{
	public class BoostProducerTests
	{
        private IBoostProvider _provider;
        private IBoostConsumer _consumer1, _consumer2;

		[SetUp]
		public void SetuUp()
		{
            _provider = new BoostProvider(boostMultiplier: 1.8f);

			_consumer1 = Substitute.For<IBoostConsumer>();
			_consumer2 = Substitute.For<IBoostConsumer>();

			UnityAsserts.Assert.raiseExceptions = true;
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
        public void Clear_RemovesSelfFromAllConsumers()
        {
			_provider.AddBoostConsumer(_consumer1);
			_consumer1.Received().AddBoostProvider(_provider);

			_provider.AddBoostConsumer(_consumer2);
			_consumer2.Received().AddBoostProvider(_provider);

            _provider.ClearBoostConsumers();
			_consumer1.Received().RemoveBoostProvider(_provider);
			_consumer2.Received().RemoveBoostProvider(_provider);
        }
	}
}
