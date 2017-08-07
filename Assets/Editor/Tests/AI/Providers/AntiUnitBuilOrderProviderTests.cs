using System.Linq;
using System.Collections.Generic;
using BattleCruisers.AI.Providers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Providers
{
    public class AntiUnitBuildOrderProviderTests
	{
        public class DummyAntiUnitBuildOrderProvider : AntiUnitBuildOrderProvider
        {
            public int NumOfSlotsToUse { get; set; }

            public DummyAntiUnitBuildOrderProvider(IStaticData staticData, IPrefabKey basicDefenceKey, IPrefabKey advancedDefenceKey) 
                : base(staticData, basicDefenceKey, advancedDefenceKey)
            {
            }

            protected override int FindNumOfSlotsToUse(int numOfDeckSlots)
            {
                return NumOfSlotsToUse;
            }
        }

        private DummyAntiUnitBuildOrderProvider _buildOrderProvider;

        private IStaticData _staticData;
        private IPrefabKey _basicDefenceKey, _advancedDefenceKey;

        [SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;

            _staticData = Substitute.For<IStaticData>();
            _basicDefenceKey = Substitute.For<IPrefabKey>();
            _advancedDefenceKey = Substitute.For<IPrefabKey>();

            _buildOrderProvider = new DummyAntiUnitBuildOrderProvider(_staticData, _basicDefenceKey, _advancedDefenceKey);
		}

		[Test]
		public void SingleSlot()
		{
            _buildOrderProvider.NumOfSlotsToUse = 1;
            IList<IPrefabKey> buildOrder = _buildOrderProvider.CreateBuildOrder(numOfDeckSlots: 12, levelNum: 12);

            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _basicDefenceKey }));
		}

		[Test]
		public void TwoSlots_AdvandedDefenceIsAvailable()
		{
			_buildOrderProvider.NumOfSlotsToUse = 2;
            int levelNum = 12;
            _staticData.IsBuildableAvailable(_advancedDefenceKey, levelNum).Returns(true);
            IList<IPrefabKey> buildOrder = _buildOrderProvider.CreateBuildOrder(numOfDeckSlots: 12, levelNum: levelNum);

            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _basicDefenceKey, _advancedDefenceKey }));
		}

		[Test]
		public void TwoSlots_AdvandedDefenceIsNotAvailable()
		{
			_buildOrderProvider.NumOfSlotsToUse = 2;
			int levelNum = 12;
			_staticData.IsBuildableAvailable(_advancedDefenceKey, levelNum).Returns(false);
			IList<IPrefabKey> buildOrder = _buildOrderProvider.CreateBuildOrder(numOfDeckSlots: 12, levelNum: levelNum);

            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _basicDefenceKey, _basicDefenceKey}));
		}

        [Test]
        public void LostOfSlots_AdvandedDefenceIsAvailable()
		{
			_buildOrderProvider.NumOfSlotsToUse = 5;
			int levelNum = 12;
			_staticData.IsBuildableAvailable(_advancedDefenceKey, levelNum).Returns(true);
			IList<IPrefabKey> buildOrder = _buildOrderProvider.CreateBuildOrder(numOfDeckSlots: 12, levelNum: levelNum);

			Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _basicDefenceKey, _advancedDefenceKey, _advancedDefenceKey, _advancedDefenceKey, _advancedDefenceKey }));
		}

        [Test]
        public void LostOfSlots_AdvandedDefenceIsNotAvailable()
        {
            _buildOrderProvider.NumOfSlotsToUse = 5;
            int levelNum = 12;
            _staticData.IsBuildableAvailable(_advancedDefenceKey, levelNum).Returns(false);
            IList<IPrefabKey> buildOrder = _buildOrderProvider.CreateBuildOrder(numOfDeckSlots: 12, levelNum: levelNum);

            Assert.IsTrue(buildOrder.SequenceEqual(new IPrefabKey[] { _basicDefenceKey, _basicDefenceKey, _basicDefenceKey, _basicDefenceKey, _basicDefenceKey }));
        }

		[Test]
		public void TooSmallSlotNum_Throws()
		{
			_buildOrderProvider.NumOfSlotsToUse = 0;
            Assert.Throws<UnityAsserts.AssertionException>(() => _buildOrderProvider.CreateBuildOrder(numOfDeckSlots: 12, levelNum: 12));
		}

        [Test]
        public void TooBigSlotNum_Throws()
        {
            _buildOrderProvider.NumOfSlotsToUse = 14;
            Assert.Throws<UnityAsserts.AssertionException>(() => _buildOrderProvider.CreateBuildOrder(numOfDeckSlots: 12, levelNum: 12));
        }
	}
}
