using System.Collections.Generic;
using BattleCruisers.AI.Providers;
using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Providers
{
    public class OffensiveBuildOrderProviderTests
    {
        private OffensiveBuildOrderProvider _provider;

        private IOffensiveRequest _highNavalRequest, _highAirRequest, _lowOffensiveRequest, _highUltrasRequest;
        private IPrefabKey _navalKey;
        private IBuildingKeyProvider _buildingKeyProvider;
        private int _numOfPlatformSlots;

        [SetUp]
        public void SetuUp()
        {
            _provider = new OffensiveBuildOrderProvider();

            _highNavalRequest = new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High, Substitute.For<IBuildingKeyProvider>());
            _highNavalRequest.BuildingKeyProvider.Next.Returns(_navalKey);
        }

        [Test]
        public void HighNaval()
        {
			_numOfPlatformSlots = 0;
			IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, _highNavalRequest);

            Assert.AreEqual(1, buildOrder.Count);
            Assert.AreSame(_navalKey, buildOrder[0]);
        }
    }
}
