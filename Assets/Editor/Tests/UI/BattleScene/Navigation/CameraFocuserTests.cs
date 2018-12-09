using BattleCruisers.UI.BattleScene.Navigation;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.BattleScene.Navigation
{
    public class CameraFocuserTests
    {
        private ICameraFocuser _cameraFocuser;
        private INavigationWheelPositionProvider _positionProvider;
        private INavigationWheel _navigationWheel;

        [SetUp]
        public void TestSetup()
        {
            _positionProvider = Substitute.For<INavigationWheelPositionProvider>();
            _navigationWheel = Substitute.For<INavigationWheel>();
            _cameraFocuser = new CameraFocuser(_positionProvider, _navigationWheel);

            _positionProvider.PlayerCruiserPosition.Returns(new Vector2(7, 7));
            _positionProvider.AICruiserPosition.Returns(new Vector2(-3, -3));
            _positionProvider.MidLeftPosition.Returns(new Vector2(-1, 1));
        }

        [Test]
        public void FocusOnPlayerCruiser()
        {
            _cameraFocuser.FocusOnPlayerCruiser();
            _navigationWheel.Received().CenterPosition = _positionProvider.PlayerCruiserPosition;
        }

        [Test]
        public void FocusOnAiCruiser()
        {
            _cameraFocuser.FocusOnAICruiser();
            _navigationWheel.Received().CenterPosition = _positionProvider.AICruiserPosition;
        }

        [Test]
        public void FocusOnMideLeft()
        {
            _cameraFocuser.FocusMidLeft();
            _navigationWheel.Received().CenterPosition = _positionProvider.MidLeftPosition;
        }
    }
}