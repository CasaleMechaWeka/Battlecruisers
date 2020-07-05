using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class SwipeCameraTargetProviderTests
    {
        private IUserInputCameraTargetProvider _targetProvider;

        private IDragTracker _dragTracker;
        private IScrollCalculator _scrollCalculator;
        private IZoomCalculator _zoomCalculator;
        private ICamera _camera;
        private ICameraCalculator _cameraCalculator;
        private IDirectionalZoom _directionalZoom;
        private IScrollRecogniser _scrollRecogniser;
        private IClamper _cameraXPositionClamper;

        private int _inputEndedCount;
        private IPointerEventData _pointerEventData;
        private DragEventArgs _dragEventArgs;

        [SetUp]
        public void TestSetup()
        {
            _dragTracker = Substitute.For<IDragTracker>();
            _scrollCalculator = Substitute.For<IScrollCalculator>();
            _zoomCalculator = Substitute.For<IZoomCalculator>();
            _camera = Substitute.For<ICamera>();
            _cameraCalculator = Substitute.For<ICameraCalculator>();
            _directionalZoom = Substitute.For<IDirectionalZoom>();
            _scrollRecogniser = Substitute.For<IScrollRecogniser>();
            _cameraXPositionClamper = Substitute.For<IClamper>();

            _targetProvider
                = new SwipeCameraTargetProvider(
                    _dragTracker,
                    _scrollCalculator,
                    _zoomCalculator,
                    _camera,
                    _cameraCalculator,
                    _directionalZoom,
                    _scrollRecogniser,
                    _cameraXPositionClamper);

            _inputEndedCount = 0;
            _targetProvider.UserInputEnded += (sender, e) => _inputEndedCount++;

            _camera.Position.Returns(new Vector3(4, 8, 12));
            _camera.OrthographicSize.Returns(-17.3f);

            _pointerEventData = Substitute.For<IPointerEventData>();
            _pointerEventData.Delta.Returns(new Vector2(1.2f, 2.1f));
            _pointerEventData.Position.Returns(new Vector2(71.9f, 91.7f));

            _dragEventArgs = new DragEventArgs(_pointerEventData);
        }

        [Test]
        public void DragEnd_NotDuringInput_DoesNotRaiseEvent()
        {
            _dragTracker.DragEnd += Raise.EventWith(_dragEventArgs);
            Assert.AreEqual(0, _inputEndedCount);
        }

        [Test]
        public void DragEnd_DuringInput_RaisesEvent()
        {
            StartUserInput();

            _dragTracker.DragEnd += Raise.EventWith(_dragEventArgs);
            Assert.AreEqual(1, _inputEndedCount);
        }

        [Test]
        public void Drag_IsScroll()
        {
            _scrollRecogniser.IsScroll(_pointerEventData.Delta).Returns(true);

            float cameraDeltaX = -7.7f;
            _scrollCalculator.FindScrollDelta(_pointerEventData.Delta.x).Returns(cameraDeltaX);

            float targetXPosition = _camera.Position.x + cameraDeltaX;

            IRange<float> validXPositions = new Range<float>(17, 71);
            _cameraCalculator.FindValidCameraXPositions(_camera.OrthographicSize).Returns(validXPositions);

            float clampedXPosition = 33.77f;
            _cameraXPositionClamper.Clamp(targetXPosition, validXPositions).Returns(clampedXPosition);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(clampedXPosition, _camera.Position.y, _camera.Position.z),
                    _camera.OrthographicSize);

            _dragTracker.Drag += Raise.EventWith(_dragEventArgs);

            Assert.AreEqual(expectedTarget, _targetProvider.Target);
        }

        [Test]
        public void Drag_IsZoom_ZoomIn()
        {
            _pointerEventData.Delta.Returns(new Vector2(0, 1));
            _scrollRecogniser.IsScroll(_pointerEventData.Delta).Returns(false);

            float orthographicSizeDelta = 48.5f;
            _zoomCalculator.FindMouseScrollOrthographicSizeDelta(_pointerEventData.Delta.y).Returns(orthographicSizeDelta);

            ICameraTarget zoomInTarget = Substitute.For<ICameraTarget>();
            _directionalZoom.ZoomIn(orthographicSizeDelta, _pointerEventData.Position).Returns(zoomInTarget);

            _dragTracker.Drag += Raise.EventWith(_dragEventArgs);

            Assert.AreSame(zoomInTarget, _targetProvider.Target);
        }

        [Test]
        public void Drag_IsZoom_ZoomOut()
        {
            _pointerEventData.Delta.Returns(new Vector2(0, -1));
            _scrollRecogniser.IsScroll(_pointerEventData.Delta).Returns(false);

            float orthographicSizeDelta = 48.5f;
            _zoomCalculator.FindMouseScrollOrthographicSizeDelta(_pointerEventData.Delta.y).Returns(orthographicSizeDelta);

            ICameraTarget zoomOutTarget = Substitute.For<ICameraTarget>();
            _directionalZoom.ZoomOut(orthographicSizeDelta).Returns(zoomOutTarget);

            _dragTracker.Drag += Raise.EventWith(_dragEventArgs);

            Assert.AreSame(zoomOutTarget, _targetProvider.Target);
        }

        private void StartUserInput()
        {
            Drag_IsScroll();
        }
    }
}