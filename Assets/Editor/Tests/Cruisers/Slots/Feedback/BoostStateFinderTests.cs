using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots.Feedback;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Slots.Feedback
{
    public class BoostStateFinderTests
    {
        private IBoostStateFinder _stateFinder;
        private IBuilding _building;

        [SetUp]
        public void TestSetup()
        {
            _stateFinder = new BoostStateFinder();
            _building = Substitute.For<IBuilding>();
        }

        [Test]
        public void FindState_NegativeNumOfBoosters_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _stateFinder.FindState(numOfLocalBoosters: -1, building: null));
        }

        [Test]
        public void FindState_NullBuilding_Off()
        {
            Assert.AreEqual(BoostState.Off, _stateFinder.FindState(numOfLocalBoosters: 99, building: null));
        }

        [Test]
        public void FindState_NonNullBuilding_NotBoostable_Off()
        {
            _building.IsBoostable.Returns(false);
            Assert.AreEqual(BoostState.Off, _stateFinder.FindState(numOfLocalBoosters: 99, building: _building));
        }

        [Test]
        public void FindState_NonNullBuilding_Boostable_0Boosters_Off()
        {
            _building.IsBoostable.Returns(true);
            Assert.AreEqual(BoostState.Off, _stateFinder.FindState(numOfLocalBoosters: 0, building: _building));
        }

        [Test]
        public void FindState_NonNullBuilding_Boostable_1Boosters_Single()
        {
            _building.IsBoostable.Returns(true);
            Assert.AreEqual(BoostState.Single, _stateFinder.FindState(numOfLocalBoosters: 1, building: _building));
        }

        [Test]
        public void FindState_NonNullBuilding_Boostable_2Boosters_Double()
        {
            _building.IsBoostable.Returns(true);
            Assert.AreEqual(BoostState.Double, _stateFinder.FindState(numOfLocalBoosters: 2, building: _building));
        }
    }
}