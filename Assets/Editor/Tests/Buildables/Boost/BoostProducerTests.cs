using BattleCruisers.Buildables.Boost;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Boost
{
	public class BoostProducerTests
	{
        private IBoostProvider _provider;
        private IBoostUser _user1, _user2;

		[SetUp]
		public void SetuUp()
		{
            _provider = new BoostProvider(boostMultiplier: 1.8f);

            _user1 = Substitute.For<IBoostUser>();
            _user2 = Substitute.For<IBoostUser>();

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
            _provider.AddBoostUser(_user1);
            _user1.Received().AddBoostProvider(_provider);
        }

        [Test]
        public void Clear_RemovesSelfFromAllConsumers()
        {
			_provider.AddBoostUser(_user1);
			_user1.Received().AddBoostProvider(_provider);

			_provider.AddBoostUser(_user2);
			_user2.Received().AddBoostProvider(_provider);

            _provider.ClearBoostUsers();
			_user1.Received().RemoveBoostProvider(_provider);
			_user2.Received().RemoveBoostProvider(_provider);
        }
	}
}
