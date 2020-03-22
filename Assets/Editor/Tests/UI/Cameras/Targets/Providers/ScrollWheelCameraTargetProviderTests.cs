using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class ScrollWheelCameraTargetProviderTests
    {
        private IUserInputCameraTargetProvider _cameraTargetProvider;

        private IInput _input;
        private IUpdater _updater;
        private IZoomCalculator _zoomCalculator;
        private IDirectionalZoom _directionalZoom;

        private int _targetChangedCount, _userInputStartedCount, _userInputEndedCount;
        private float _orthograhpicSizeDelta = 7.23f;
        private ICameraTarget _expectedTarget;

        [SetUp]
        public void TestSetup()
        {
            _input = Substitute.For<IInput>();
            _updater = Substitute.For<IUpdater>();
            _zoomCalculator = Substitute.For<IZoomCalculator>();
            _directionalZoom = Substitute.For<IDirectionalZoom>();

            _cameraTargetProvider
                = new ScrollWheelCameraTargetProvider(
                    _input,
                    _updater,
                    _zoomCalculator,
                    _directionalZoom);

            _targetChangedCount = 0;
            _cameraTargetProvider.TargetChanged += (sender, e) => _targetChangedCount++;

            _userInputStartedCount = 0;
            _cameraTargetProvider.UserInputStarted += (sender, e) => _userInputStartedCount++;

            _userInputEndedCount = 0;
            _cameraTargetProvider.UserInputEnded += (sender, e) => _userInputEndedCount++;

            _zoomCalculator.FindMouseScrollOrthographicSizeDelta(default).ReturnsForAnyArgs(_orthograhpicSizeDelta);

            _input.MousePosition.Returns(new Vector3(99, 98, 97));

            _expectedTarget = new CameraTarget(new Vector3(0.1f, -0.1f, 0.2f), -12.12f);
            _directionalZoom.ZoomOut(_orthograhpicSizeDelta).Returns(_expectedTarget);
            _directionalZoom.ZoomIn(_orthograhpicSizeDelta, _input.MousePosition).Returns(_expectedTarget);
        }

        [Test]
        public void InitialState()
        {
            Assert.IsNull(_cameraTargetProvider.Target);
        }

        [Test]
        public void Update_StarteUserInput()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputStartedCount);
        }

        [Test]
        public void Update_UserInput_DuringUserInput()
        {
            // First user input
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputStartedCount);

            // Second user input, event not raised again
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputStartedCount);
        }

        [Test]
        public void Update_NoUserInput()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, 0));

            _updater.Updated += Raise.Event();

            Assert.AreEqual(0, _targetChangedCount);
            Assert.AreEqual(0, _userInputEndedCount);
        }

        [Test]
        public void Update_NoUserInput_DuringUserInput()
        {
            // Start user input
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputStartedCount);

            // End user input
            _input.MouseScrollDelta.Returns(new Vector2(0, 0));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputEndedCount);
        }

        [Test]
        public void Update_ZoomOut()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, -1));

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(_expectedTarget, _cameraTargetProvider.Target);
        }

        [Test]
        public void Update_ZoomIn()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(_expectedTarget, _cameraTargetProvider.Target);
        }
    }
}