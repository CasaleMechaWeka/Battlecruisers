using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Helpers
{
    public class PlayerCruiserFocusHelperTests
    {
        private IPlayerCruiserFocusHelper _helper;
        private ICamera _camera;
        private ICameraFocuser _cameraFocuser;
        private ICruiser _playerCruiser;
        private ISlot _bowSlot;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _cameraFocuser = Substitute.For<ICameraFocuser>();
            _playerCruiser = Substitute.For<ICruiser>();

            _helper = new PlayerCruiserFocusHelper(_camera, _cameraFocuser, _playerCruiser);

            _bowSlot = Substitute.For<ISlot>();
            ReadOnlyCollection<ISlot> bowSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>() { _bowSlot });
            _playerCruiser.SlotAccessor
                .GetSlots(SlotType.Bow)
                .Returns(bowSlots);
        }

        [Test]
        public void FocusOnPlayerCruiserIfNeeded_CameraRoughlyOnCruiser_DoesNotMoveCamera()
        {
            _playerCruiser.Position.Returns(Vector2.zero);
            _camera.Transform.Position.Returns(new Vector3(PlayerCruiserFocusHelper.PLAYER_CRUISER_CAMERA_MARGIN_IN_M - 0.1f, 0, 0));

            _helper.FocusOnPlayerCruiserIfNeeded();

            _cameraFocuser.DidNotReceive().FocusOnPlayerCruiser();
        }

        [Test]
        public void FocusOnPlayerCruiserIfNeeded_CameraNotRoughlyOnCruiser_MovesCamera()
        {
            _playerCruiser.Position.Returns(Vector2.zero);
            _camera.Transform.Position.Returns(new Vector3(PlayerCruiserFocusHelper.PLAYER_CRUISER_CAMERA_MARGIN_IN_M, 0, 0));

            _helper.FocusOnPlayerCruiserIfNeeded();

            _cameraFocuser.Received().FocusOnPlayerCruiser();
        }

        [Test]
        public void FocusOnPlayerBowSlotIfNeeded_CameraRoughlyOnBowSlot_DoesNotMoveCamera()
        {
            _bowSlot.Position.Returns(Vector2.zero);
            _camera.Transform.Position.Returns(new Vector3(PlayerCruiserFocusHelper.BOW_SLOT_CAMERA_MARGIN_IN_M - 0.1f, 0, 0));

            _helper.FocusOnPlayerBowSlotIfNeeded();

            _cameraFocuser.DidNotReceive().FocusOnPlayerNavalFactory();
        }

        [Test]
        public void FocusOnPlayerBowSlotIfNeeded_CameraNOtRoughlyOnBowSlot_MovesCamera()
        {
            _bowSlot.Position.Returns(Vector2.zero);
            _camera.Transform.Position.Returns(new Vector3(PlayerCruiserFocusHelper.BOW_SLOT_CAMERA_MARGIN_IN_M, 0, 0));

            _helper.FocusOnPlayerBowSlotIfNeeded();

            _cameraFocuser.Received().FocusOnPlayerNavalFactory();
        }
    }
}