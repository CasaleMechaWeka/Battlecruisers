using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class PinchZoomCameraTargetProviderTests
    {
        private UserInputCameraTargetProvider _targetProvider;
        private ZoomCalculator _zoomCalculator;
        private IDirectionalZoom _directionalZoom;
        private PinchTracker _pinchTracker;
        private int _endedCount;
        private CameraTarget _target;

        [SetUp]
        public void TestSetup()
        {
            _zoomCalculator = Substitute.For<ZoomCalculator>();
            _directionalZoom = Substitute.For<IDirectionalZoom>();
            _pinchTracker = Substitute.For<PinchTracker>();

            _targetProvider = new PinchZoomCameraTargetProvider(_zoomCalculator, _directionalZoom, _pinchTracker);

            _endedCount = 0;
            _targetProvider.UserInputEnded += (sender, e) => _endedCount++;

            _target = Substitute.For<CameraTarget>();
        }

        [Test]
        public void _pinchTracker_Pinch_NegativeDelta()
        {
            PinchEventArgs pinchEventArgs = new PinchEventArgs(position: new Vector2(3, 2), deltaInM: -12);
            float orthographicSizeDelta = 13;
            _zoomCalculator.FindPinchZoomOrthographicSizeDelta(pinchEventArgs.DeltaInM).Returns(orthographicSizeDelta);
            _directionalZoom.ZoomOut(orthographicSizeDelta).Returns(_target);

            _pinchTracker.Pinch += Raise.EventWith(pinchEventArgs);

            Assert.AreSame(_target, _targetProvider.Target);
        }

        [Test]
        public void _pinchTracker_Pinch_PositiveDelta()
        {
            PinchEventArgs pinchEventArgs = new PinchEventArgs(position: new Vector2(3, 2), deltaInM: 12);
            float orthographicSizeDelta = 13;
            _zoomCalculator.FindPinchZoomOrthographicSizeDelta(pinchEventArgs.DeltaInM).Returns(orthographicSizeDelta);
            _directionalZoom.ZoomIn(orthographicSizeDelta, pinchEventArgs.Position).Returns(_target);

            _pinchTracker.Pinch += Raise.EventWith(pinchEventArgs);

            Assert.AreSame(_target, _targetProvider.Target);
        }

        [Test]
        public void _pinchTracker_PinchEnd()
        {
            StartUserInput();

            _pinchTracker.PinchEnd += Raise.Event();
            Assert.AreEqual(1, _endedCount);
        }

        private void StartUserInput()
        {
            _pinchTracker_Pinch_NegativeDelta();
        }
    }
}