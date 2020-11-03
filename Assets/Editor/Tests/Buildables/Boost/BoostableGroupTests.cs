using BattleCruisers.Buildables.Boost;
using NSubstitute;
using NUnit.Framework;
using System.Collections.ObjectModel;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Boost
{
    public class BoostableGroupTests
	{
        private IBoostableGroup _group;
        private IBoostConsumer _consumer;
        private IBoostable _boostable1, _boostable2;
        private ObservableCollection<IBoostProvider> _providers1, _providers2;
        private IBoostProvider _provider1, _provider2;
        private IBoostFactory _factory;

		[SetUp]
		public void SetuUp()
		{
            _consumer = Substitute.For<IBoostConsumer>();
            _consumer.CumulativeBoost.Returns(1.17f);

            _factory = Substitute.For<IBoostFactory>();
            _factory.CreateBoostConsumer().Returns(_consumer);

            _group = new BoostableGroup(_factory);
			
            _boostable1 = Substitute.For<IBoostable>();
			_boostable2 = Substitute.For<IBoostable>();

            _provider1 = Substitute.For<IBoostProvider>();
			_providers1 = new ObservableCollection<IBoostProvider>();
            _providers1.Add(_provider1);

            _provider2 = Substitute.For<IBoostProvider>();
			_providers2 = new ObservableCollection<IBoostProvider>();
            _providers2.Add(_provider2);
		}

        #region Boostables
        [Test]
		public void AddBoostable_SetsBoost()
		{
            _group.AddBoostable(_boostable1);
            _boostable1.Received().BoostMultiplier = _consumer.CumulativeBoost;
		}

        [Test]
        public void AddDuplicateBoostable_Throws()
        {
			_group.AddBoostable(_boostable1);
            Assert.Throws<UnityAsserts.AssertionException>(() => _group.AddBoostable(_boostable1));
		}

        [Test]
        public void RemoveBoostable_DoesNotThrow()
        {
            _group.AddBoostable(_boostable1);
            _group.RemoveBoostable(_boostable1);
        }

        [Test]
        public void RemoveNonExistantBoostable_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _group.RemoveBoostable(_boostable1));
        }
		#endregion Boostables

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

        #region BoostProviders
        [Test]
        public void AddBoostProviders()
        {
            _group.AddBoostProvidersList(_providers1);
            _provider1.Received().AddBoostConsumer(_consumer);
        }

        [Test]
        public void AddMultipleBoostProviders()
        {
			_group.AddBoostProvidersList(_providers1);
			_provider1.Received().AddBoostConsumer(_consumer);

			_group.AddBoostProvidersList(_providers2);
			_provider2.Received().AddBoostConsumer(_consumer);
        }

        [Test]
        public void BoostProviderAdded_AddsBoostConsumer()
        {
			_group.AddBoostProvidersList(_providers1);
			_provider1.Received().AddBoostConsumer(_consumer);

            _providers1.Add(_provider2);
			_provider2.Received().AddBoostConsumer(_consumer);
        }

        [Test]
        public void BoostProviderRemoved_RemovesBoostConsumer()
        {
            _group.AddBoostProvidersList(_providers1);
            _provider1.Received().AddBoostConsumer(_consumer);
			
            _providers1.Remove(_provider1);
            _provider1.Received().RemoveBoostConsumer(_consumer);
		}
		#endregion BoostProviders

        [Test]
        public void BoostConsumer_BoostChanged_FiresEvent()
        {
            int changeCount = 0;
            _group.BoostChanged += (sender, e) => changeCount++;
            _consumer.BoostChanged += Raise.Event();
            Assert.AreEqual(1, changeCount);
        }

        [Test]
        public void CleanUp_RemovesBoostConsumerFromAllBoostProviders()
        {
			_group.AddBoostProvidersList(_providers1);
			_provider1.Received().AddBoostConsumer(_consumer);

			_group.AddBoostProvidersList(_providers2);
			_provider2.Received().AddBoostConsumer(_consumer);

            _group.CleanUp();

			_provider1.Received().RemoveBoostConsumer(_consumer);
			_provider2.Received().RemoveBoostConsumer(_consumer);
        }

        [Test]
        public void DoubleCleanUp_Throws()
        {
            _group.CleanUp();
            Assert.Throws<UnityAsserts.AssertionException>(() => _group.CleanUp());
        }
	}
}
