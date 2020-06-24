using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
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
        private IStaticCameraTargetProvider _trumpCameraTargetProvider;
        private ICameraTarget _playerCruiserCameraTarget, _playerCruiserDeathCameraTarget, _aiCruiserCameraTarget, _aiCruiserDeathCameraTarget;

        [SetUp]
        public void TestSetup()
        {
            _positionProvider = Substitute.For<INavigationWheelPositionProvider>();
            _navigationWheel = Substitute.For<INavigationWheel>();
            _trumpCameraTargetProvider = Substitute.For<IStaticCameraTargetProvider>();

        // FELIX  Fix :)
            _cameraFocuser = new CameraFocuser(_positionProvider, _trumpCameraTargetProvider, null);

            //_positionProvider.PlayerCruiserTarget.Returns(new Vector2(7, 7));
            //_positionProvider.PlayerNavalFactoryTarget.Returns(new Vector2(4, 4));
            //_positionProvider.AICruiserTarget.Returns(new Vector2(-3, -3));
            //_positionProvider.AINavalFactoryTarget.Returns(new Vector2(-9, 9));
            //_positionProvider.MidLeftTarget.Returns(new Vector2(-1, 1));
            //_positionProvider.OverviewTarget.Returns(new Vector2(762, 681));

            _playerCruiserCameraTarget = Substitute.For<ICameraTarget>();
            _positionProvider.PlayerCruiserNukedTarget.Returns(_playerCruiserCameraTarget);

            _playerCruiserDeathCameraTarget = Substitute.For<ICameraTarget>();
            _positionProvider.PlayerCruiserDeathTarget.Returns(_playerCruiserDeathCameraTarget);

            _aiCruiserCameraTarget = Substitute.For<ICameraTarget>();
            _positionProvider.AICruiserNukedTarget.Returns(_aiCruiserCameraTarget);

            _aiCruiserDeathCameraTarget = Substitute.For<ICameraTarget>();
            _positionProvider.AICruiserDeathTarget.Returns(_aiCruiserDeathCameraTarget);
        }

        [Test]
        public void FocusOnPlayerCruiser()
        {
            _cameraFocuser.FocusOnPlayerCruiser();
            //_navigationWheel.Received().SetCenterPosition(_positionProvider.PlayerCruiserTarget, snapToCorners: true);
        }

        [Test]
        public void FocusOnPlayerCruiserZoomedOut()
        {
            _cameraFocuser.FocusOnPlayerCruiserDeath();
            //_trumpCameraTargetProvider.Received().SetTarget(_positionProvider.PlayerCruiserDeathTarget);
        }

        [Test]
        public void FocusOnPlayerNavalFactory()
        {
            _cameraFocuser.FocusOnPlayerNavalFactory();
            //_navigationWheel.Received().SetCenterPosition(_positionProvider.PlayerNavalFactoryTarget, snapToCorners: false);
        }

        [Test]
        public void FocusOnAICruiser()
        {
            _cameraFocuser.FocusOnAICruiser();
            //_navigationWheel.Received().SetCenterPosition(_positionProvider.AICruiserTarget, snapToCorners: true);
        }

        [Test]
        public void FocusOnAICruiserZoomedOut()
        {
            _cameraFocuser.FocusOnAICruiserDeath();
            _trumpCameraTargetProvider.Received().SetTarget(_positionProvider.AICruiserDeathTarget);
        }

        [Test]
        public void FocusOnAINavalFactory()
        {
            _cameraFocuser.FocusOnAINavalFactory();
            //_navigationWheel.Received().SetCenterPosition(_positionProvider.AINavalFactoryTarget, snapToCorners: false);
        }

        [Test]
        public void FocusOnMidLeft()
        {
            _cameraFocuser.FocusMidLeft();
            //_navigationWheel.Received().SetCenterPosition(_positionProvider.MidLeftTarget, snapToCorners: true);
        }

        [Test]
        public void FocusOnOverview()
        {
            _cameraFocuser.FocusOnOverview();
            //_navigationWheel.Received().SetCenterPosition(_positionProvider.OverviewTarget, snapToCorners: true);
        }

        [Test]
        public void FocusOnPlayerCruiserNuke()
        {
            _cameraFocuser.FocusOnPlayerCruiserNuke();
            _trumpCameraTargetProvider.Received().SetTarget(_playerCruiserCameraTarget);
        }

        [Test]
        public void FocusOnAICruiserNuke()
        {
            _cameraFocuser.FocusOnAICruiserNuke();
            _trumpCameraTargetProvider.Received().SetTarget(_aiCruiserCameraTarget);
        }
    }
}