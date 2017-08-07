using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.Providers;
using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.AI.Providers.Strategies;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Providers
{
    public class OffensiveBuildOrderProviderTests
    {
        private OffensiveBuildOrderProvider _provider;

        private IOffensiveRequest _lowNavalRequest, _highNavalRequest, _lowAirRequest, _highAirRequest, _lowOffensiveRequest, _highUltrasRequest;
        private IPrefabKey _navalKey, _airKey, _offensiveKey, _ultrasKey;
        private int _numOfPlatformSlots;

        [SetUp]
        public void SetuUp()
        {
            _provider = new OffensiveBuildOrderProvider();


            // Naval
            _navalKey = Substitute.For<IPrefabKey>();
			
			_lowNavalRequest = new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low, Substitute.For<IBuildingKeyProvider>());
			_lowNavalRequest.BuildingKeyProvider.Next.Returns(_navalKey);

            _highNavalRequest = new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High, Substitute.For<IBuildingKeyProvider>());
            _highNavalRequest.BuildingKeyProvider.Next.Returns(_navalKey);


            // Air
            _airKey = Substitute.For<IPrefabKey>();

            _lowAirRequest = new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low, Substitute.For<IBuildingKeyProvider>());
            _lowAirRequest.BuildingKeyProvider.Next.Returns(_airKey);

            _highAirRequest = new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High, Substitute.For<IBuildingKeyProvider>());
            _highAirRequest.BuildingKeyProvider.Next.Returns(_airKey);


            // Offensive
            _offensiveKey = Substitute.For<IPrefabKey>();

            _lowOffensiveRequest = new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low, Substitute.For<IBuildingKeyProvider>());
            _lowOffensiveRequest.BuildingKeyProvider.Next.Returns(_offensiveKey);


            // Ultras
            _ultrasKey = Substitute.For<IPrefabKey>();

            _highUltrasRequest = new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High, Substitute.For<IBuildingKeyProvider>());
            _highUltrasRequest.BuildingKeyProvider.Next.Returns(_ultrasKey);
        }

		[Test]
		public void LowNaval()
		{
			_numOfPlatformSlots = 0;
            IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _lowNavalRequest });

			Assert.AreEqual(1, buildOrder.Count);
            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _navalKey }));
		}

        [Test]
        public void HighNaval()
        {
			_numOfPlatformSlots = 0;
			IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _highNavalRequest });

            Assert.AreEqual(1, buildOrder.Count);
			Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _navalKey }));
        }

        [Test]
        public void LowNaval_HighOther()
        {
            _numOfPlatformSlots = 0;
            IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _lowNavalRequest, _highAirRequest });

            Assert.AreEqual(1, buildOrder.Count);
			Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _navalKey }));
        }

        #region High_Low
        [Test]
		public void High_Low_1Slot()
		{
			_numOfPlatformSlots = 1;
            IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest });

			Assert.AreEqual(1, buildOrder.Count);
            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _airKey }));
		}

		[Test]
		public void High_Low_2Slots()
		{
			_numOfPlatformSlots = 2;
			IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest });

			Assert.AreEqual(2, buildOrder.Count);
            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _airKey, _offensiveKey }));
		}

		[Test]
		public void High_Low_3Slots()
		{
			_numOfPlatformSlots = 3;
			IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest });

			Assert.AreEqual(3, buildOrder.Count);
            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _airKey, _airKey, _offensiveKey }));
		}
	
        [Test]
		public void High_Low_4Slots()
		{
			_numOfPlatformSlots = 4;
			IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest });

			Assert.AreEqual(4, buildOrder.Count);
            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _airKey, _airKey, _airKey, _offensiveKey }));
		}
		#endregion High_Low

		[Test]
		public void High_Low_Low_4Slots()
		{
			_numOfPlatformSlots = 4;
            IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _lowAirRequest, _lowOffensiveRequest, _highUltrasRequest });

			Assert.AreEqual(4, buildOrder.Count);
            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _airKey, _offensiveKey, _ultrasKey, _ultrasKey}));
        }
		
		[Test]
		public void High_High_Low_2Slots()
		{
			_numOfPlatformSlots = 2;
			IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest, _highUltrasRequest });
			
			Assert.AreEqual(2, buildOrder.Count);
			Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _airKey, _ultrasKey }));
		}

        [Test]
        public void High_High_Low_4Slots()
        {
            _numOfPlatformSlots = 4;
            IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest, _highUltrasRequest });

            Assert.AreEqual(4, buildOrder.Count);
            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _airKey, _airKey, _offensiveKey, _ultrasKey }));
        }

        [Test]
        public void High_High_Low_5Slots()
        {
            _numOfPlatformSlots = 5;
            IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _highAirRequest, _lowOffensiveRequest, _highUltrasRequest });

            Assert.AreEqual(5, buildOrder.Count);
            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _airKey, _airKey, _offensiveKey, _ultrasKey, _ultrasKey }));
        }
		
        [Test]
		public void High_High_1Slots()
		{
			_numOfPlatformSlots = 1;
            IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _highUltrasRequest, _highAirRequest });

			Assert.AreEqual(1, buildOrder.Count);
			Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _ultrasKey }));
		}

		[Test]
		public void LeftOverSlots()
		{
			_numOfPlatformSlots = 2;
			IList<IPrefabKey> buildOrder = _provider.CreateBuildOrder(_numOfPlatformSlots, new IOffensiveRequest[] { _lowAirRequest });

			Assert.AreEqual(1, buildOrder.Count);
            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _airKey }));
		}
    }
}
