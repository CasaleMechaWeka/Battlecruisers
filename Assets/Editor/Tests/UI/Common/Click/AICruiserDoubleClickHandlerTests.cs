using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.Common.Click;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Common.Click
{
    public class AICruiserDoubleClickHandlerTests
    {
        private IDoubleClickHandler<ICruiser> _handler;
        private IUserChosenTargetHelper _userChosenTargetHelper;
        private ICruiser _clickedCruiser;

        [SetUp]
        public void TestSetup()
        {
            _userChosenTargetHelper = Substitute.For<IUserChosenTargetHelper>();
            _handler = new AICruiserDoubleClickHandler(_userChosenTargetHelper);

            _clickedCruiser = Substitute.For<ICruiser>();

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void OnDoubleClick_WrongFaction_Throws()
        {
            _clickedCruiser.Faction.Returns(Faction.Blues);
            Assert.Throws<UnityAsserts.AssertionException>(() => _handler.OnDoubleClick(_clickedCruiser));
        }

        [Test]
        public void OnDoubleClick_RightFaction_SelectsTarget()
        {
            _clickedCruiser.Faction.Returns(Faction.Reds);
            _handler.OnDoubleClick(_clickedCruiser);
            _userChosenTargetHelper.Received().ToggleChosenTarget(_clickedCruiser);
        }
    }
}