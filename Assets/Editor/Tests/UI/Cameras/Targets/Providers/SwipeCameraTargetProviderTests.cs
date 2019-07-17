using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Clamping;

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

        private int _inputStartedCount, _inputEndedCount;

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

            _inputStartedCount = 0;
            _targetProvider.UserInputStarted += (sender, e) => _inputStartedCount++;

            _inputEndedCount = 0;
            _targetProvider.UserInputEnded += (sender, e) => _inputEndedCount++;
        }

        [Test]
        public void DragStart_RaisesEvent()
        {
            // FELIX :P
            //_dragTracker.DragStart += Raise.EventWith(new DragEventArgs(new UnityEngine.EventSystems.PointerEventData()))
        }

        [Test]
        public void DragEnd_RaisesEvent()
        {
        }

        [Test]
        public void Drag_IsScroll()
        {
        }

        [Test]
        public void Drag_IsZoom_ZoomIn()
        {
        }

        [Test]
        public void Drag_IsZoom_ZoomOut()
        {
        }
    }
}