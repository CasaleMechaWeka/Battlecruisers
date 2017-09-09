using BattleCruisers.Buildables.Boost;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Boost
{
	public class BoostProviderListTests
	{
        private IBoostProviderList _providers;
        private IBoostProvider _provider;
        private int _changeCount;

		[SetUp]
		public void SetuUp()
		{
            _providers = new BoostProviderList();
            _provider = Substitute.For<IBoostProvider>();
            _changeCount = 0;

            _providers.ProvidersChanged += (sender, e) => _changeCount++;

			UnityAsserts.Assert.raiseExceptions = true;
		}

		[Test]
		public void Add_AddsAndEmitsChange()
		{
            _providers.AddBoostProvider(_provider);

            Assert.IsTrue(_providers.BoostProviders.Contains(_provider));
            Assert.AreEqual(1, _changeCount);
		}

        [Test]
        public void AddDuplicate_Throws()
        {
			_providers.AddBoostProvider(_provider);
			Assert.Throws<UnityAsserts.AssertionException>(() => _providers.AddBoostProvider(_provider));
        }

        [Test]
        public void Remove_RemovesAndEmitsChange()
        {
			_providers.AddBoostProvider(_provider);

			Assert.IsTrue(_providers.BoostProviders.Contains(_provider));
			Assert.AreEqual(1, _changeCount);

            _providers.RemoveBoostProvider(_provider);

            Assert.IsFalse(_providers.BoostProviders.Contains(_provider));
            Assert.AreEqual(2, _changeCount);
        }

        [Test]
        public void RemoveNonExistant_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _providers.RemoveBoostProvider(_provider));
        }
	}
}
