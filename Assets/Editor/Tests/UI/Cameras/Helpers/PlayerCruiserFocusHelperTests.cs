using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Helpers
{
    public class PlayerCruiserFocusHelperTests
    {
        private IPlayerCruiserFocusHelper _helper;
        private ICamera _camera;
        private ICameraFocuser _cameraFocuser;
        private ICruiser _playerCruiser;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _cameraFocuser = Substitute.For<ICameraFocuser>();
            _playerCruiser = Substitute.For<ICruiser>();

            _helper = new PlayerCruiserFocusHelper(_camera, _cameraFocuser, _playerCruiser);
        }

        [Test]
        public void FocusOnPlayerCruiserIfNeeded_CameraRoughlyOnCruiser_DoesNotMoveCamera()
        {
            _playerCruiser.Position.Returns(Vector2.zero);
            _camera.Transform.Position.Returns(new Vector3(9.9f, 0, 0));

            _helper.FocusOnPlayerCruiserIfNeeded();

            _cameraFocuser.DidNotReceive().FocusOnPlayerCruiser();
        }

        [Test]
        public void FocusOnPlayerCruiserIfNeeded_CameraNotRoughlyOnCruiser_MovesCamera()
        {
            _playerCruiser.Position.Returns(Vector2.zero);
            _camera.Transform.Position.Returns(new Vector3(10, 0, 0));

            _helper.FocusOnPlayerCruiserIfNeeded();

            _cameraFocuser.Received().FocusOnPlayerCruiser();
        }
    }
}