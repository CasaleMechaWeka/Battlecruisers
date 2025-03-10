using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.Common.Click;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Common.Click
{
    public class AIBuildingDoubleClickHandlerTests
    {
        private IDoubleClickHandler<IBuilding> _handler;
        private IUserChosenTargetHelper _userChosenTargetHelper;
        private IBuilding _clickedBuilding;

        [SetUp]
        public void TestSetup()
        {
            _userChosenTargetHelper = Substitute.For<IUserChosenTargetHelper>();
            _handler = new AIBuildingDoubleClickHandler(_userChosenTargetHelper);

            _clickedBuilding = Substitute.For<IBuilding>();
        }

        [Test]
        public void OnDoubleClick_WrongFaction_Throws()
        {
            _clickedBuilding.Faction.Returns(Faction.Blues);
            Assert.Throws<UnityAsserts.AssertionException>(() => _handler.OnDoubleClick(_clickedBuilding));
        }

        [Test]
        public void OnDoubleClick_RightFaction_SelectsTarget()
        {
            _clickedBuilding.Faction.Returns(Faction.Reds);
            _handler.OnDoubleClick(_clickedBuilding);
            _userChosenTargetHelper.Received().ToggleChosenTarget(_clickedBuilding);
        }
    }
}