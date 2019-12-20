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
            _positionProvider.PlayerCruiserDeathPosition.Returns(new Vector2(77, 88));
            _positionProvider.PlayerNavalFactoryPosition.Returns(new Vector2(4, 4));
            _positionProvider.AICruiserPosition.Returns(new Vector2(-3, -3));
            _positionProvider.AICruiserDeathPosition.Returns(new Vector2(-33, -11));
            _positionProvider.AINavalFactoryPosition.Returns(new Vector2(-9, 9));
            _positionProvider.MidLeftPosition.Returns(new Vector2(-1, 1));
            _positionProvider.OverviewPosition.Returns(new Vector2(762, 681));
        }

        [Test]
        public void FocusOnPlayerCruiser()
        {
            _cameraFocuser.FocusOnPlayerCruiser();
            _navigationWheel.Received().SetCenterPosition(_positionProvider.PlayerCruiserPosition, snapToCorners: true);
        }

        [Test]
        public void FocusOnPlayerCruiserZoomedOut()
        {
            _cameraFocuser.FocusOnPlayerCruiserDeath();
            _navigationWheel.Received().SetCenterPosition(_positionProvider.PlayerCruiserDeathPosition, snapToCorners: false);
        }

        [Test]
        public void FocusOnPlayerNavalFactory()
        {
            _cameraFocuser.FocusOnPlayerNavalFactory();
            _navigationWheel.Received().SetCenterPosition(_positionProvider.PlayerNavalFactoryPosition, snapToCorners: true);
        }

        [Test]
        public void FocusOnAICruiser()
        {
            _cameraFocuser.FocusOnAICruiser();
            _navigationWheel.Received().SetCenterPosition(_positionProvider.AICruiserPosition, snapToCorners: true);
        }

        [Test]
        public void FocusOnAICruiserZoomedOut()
        {
            _cameraFocuser.FocusOnAICruiserDeath();
            _navigationWheel.Received().SetCenterPosition(_positionProvider.AICruiserDeathPosition, snapToCorners: false);
        }

        [Test]
        public void FocusOnAINavalFactory()
        {
            _cameraFocuser.FocusOnAINavalFactory();
            _navigationWheel.Received().SetCenterPosition(_positionProvider.AINavalFactoryPosition, snapToCorners: true);
        }

        [Test]
        public void FocusOnMidLeft()
        {
            _cameraFocuser.FocusMidLeft();
            _navigationWheel.Received().SetCenterPosition(_positionProvider.MidLeftPosition, snapToCorners: true);
        }

        [Test]
        public void FocusOnOverview()
        {
            _cameraFocuser.FocusOnOverview();
            _navigationWheel.Received().SetCenterPosition(_positionProvider.OverviewPosition, snapToCorners: true);
        }
    }
}