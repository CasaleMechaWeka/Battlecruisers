using BattleCruisers.Buildables.Boost;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Boost
{
	public class BoostableGroupTests
	{
        private IBoostableGroup _group;
        private IBoostConsumer _consumer;
        private IBoostable _boostable1, _boostable2;

		[SetUp]
		public void SetuUp()
		{
            _consumer = Substitute.For<IBoostConsumer>();
            _consumer.CumulativeBoost.Returns(1.17f);

            _group = new BoostableGroup(_consumer);
			
            _boostable1 = Substitute.For<IBoostable>();
			_boostable2 = Substitute.For<IBoostable>();

			UnityAsserts.Assert.raiseExceptions = true;
		}

        [Test]
        public void BoostableConsumer_ReturnsConsumer()
        {
            Assert.AreSame(_consumer, _group.BoostConsumer);
        }

		[Test]
		public void AddBoostable_SetsBoost()
		{
            _group.AddBoostable(_boostable1);
            _boostable1.Received().BoostMultiplier = _consumer.CumulativeBoost;
		}

        [Test]
        public void BoostChanged_UpdatesBoostables()
        {
			_group.AddBoostable(_boostable1);
			_group.AddBoostable(_boostable2);

            _consumer.CumulativeBoost.Returns(7.25f);
            _consumer.BoostChanged += Raise.Event();

			_boostable1.Received().BoostMultiplier = _consumer.CumulativeBoost;
			_boostable2.Received().BoostMultiplier = _consumer.CumulativeBoost;
		}
	}
}
