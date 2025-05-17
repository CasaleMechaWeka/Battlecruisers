using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class EdgeScrollingCameraTargetProviderTests
    {
        private IUserInputCameraTargetProvider _targetProvider;

        private IUpdater _updater;
        private EdgeScrollCalculator _scrollCalculator;
        private ICamera _camera;
        private CameraCalculator _cameraCalculator;
        private EdgeDetector _edgeDetector;
        private BufferClamper _cameraXPositionClamper;
        private int _inputEndedCount;
        private IRange<float> _validXPositions;
        private float _cameraDeltaX;

        [SetUp]
        public void TestSetup()
        {
            _updater = Substitute.For<IUpdater>();
            _scrollCalculator = Substitute.For<EdgeScrollCalculator>();
            _camera = Substitute.For<ICamera>();
            _cameraCalculator = Substitute.For<CameraCalculator>();
            _edgeDetector = Substitute.For<EdgeDetector>();
            _cameraXPositionClamper = Substitute.For<BufferClamper>();

            _targetProvider
                = new EdgeScrollingCameraTargetProvider(
                    _updater,
                    _scrollCalculator,
                    _camera,
                    _cameraCalculator,
                    _edgeDetector);

            _inputEndedCount = 0;
            _targetProvider.UserInputEnded += (sender, e) => _inputEndedCount++;

            _camera.Position.Returns(new Vector3(4, 8, 12));
            _camera.OrthographicSize.Returns(-17.3f);

            _validXPositions = Substitute.For<IRange<float>>();
            _cameraCalculator.FindValidCameraXPositions(_camera.OrthographicSize).Returns(_validXPositions);

            _cameraDeltaX = 7.1f;
            _scrollCalculator.FindCameraPositionDeltaMagnituteInM().Returns(_cameraDeltaX);
        }

        [Test]
        public void _updater_Updated_NoInput()
        {
            StartUserInput();

            _edgeDetector.IsCursorAtRightEdge().Returns(false);
            _edgeDetector.IsCursorAtLeftEdge().Returns(false);

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _inputEndedCount);
        }

        [Test]
        public void _updater_Updated_LeftEdge()
        {
            // Arrange
            _edgeDetector.IsCursorAtLeftEdge().Returns(true);
            _edgeDetector.IsCursorAtRightEdge().Returns(false);

            float directionMultiplier = -1;

            float targetXPosition = _camera.Position.x + (directionMultiplier * _cameraDeltaX);

            float clampedTargetXPosition = 32.1f;
            _cameraXPositionClamper.Clamp(targetXPosition, _validXPositions).Returns(clampedTargetXPosition);

            // Act
            _updater.Updated += Raise.Event();

            // Assert
            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(clampedTargetXPosition, _camera.Position.y, _camera.Position.z),
                    _camera.OrthographicSize);
            Assert.AreEqual(expectedTarget, _targetProvider.Target);
        }

        [Test]
        public void _updater_Updated_RightEdge()
        {
            // Arrange
            _edgeDetector.IsCursorAtLeftEdge().Returns(false);
            _edgeDetector.IsCursorAtRightEdge().Returns(true);

            float directionMultiplier = 1;

            float targetXPosition = _camera.Position.x + (directionMultiplier * _cameraDeltaX);

            float clampedTargetXPosition = 2.9f;
            _cameraXPositionClamper.Clamp(targetXPosition, _validXPositions).Returns(clampedTargetXPosition);

            // Act
            _updater.Updated += Raise.Event();

            // Assert
            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(clampedTargetXPosition, _camera.Position.y, _camera.Position.z),
                    _camera.OrthographicSize);
            Assert.AreEqual(expectedTarget, _targetProvider.Target);
        }

        private void StartUserInput()
        {
            _updater_Updated_LeftEdge();
        }
    }
}