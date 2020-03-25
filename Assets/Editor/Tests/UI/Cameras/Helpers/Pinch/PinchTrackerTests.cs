using BattleCruisers.UI.Cameras.Helpers.Pinch;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Helpers.Pinch
{
    public class PinchTrackerTests
    {
        private IPinchTracker _pinchTracker;
        private IInput _input;
        private IUpdater _updater;
        private PinchEventArgs _lastPinchEventArgs;
        private int _pinchStartCount, _pinchEndCount;
        private Vector2 _lastTouchPosition1, _lastTouchPosition2, _currentTouchPosition1, _currentTouchPosition2, _newestTouchPosition1, _newestTouchPosition2;

        [SetUp]
        public void TestSetup()
        {
            _input = Substitute.For<IInput>();
            _updater = Substitute.For<IUpdater>();

            _pinchTracker = new PinchTracker(_input, _updater);

            _pinchStartCount = 0;
            _pinchTracker.PinchStart += (sender, e) => _pinchStartCount++;

            _pinchEndCount = 0;
            _pinchTracker.PinchEnd += (sender, e) => _pinchEndCount++;

            _lastPinchEventArgs = null;
            _pinchTracker.Pinch += (sender, e) => _lastPinchEventArgs = e;

            _lastTouchPosition1 = new Vector2(2, 4);
            _currentTouchPosition1 = new Vector2(3, 5);
            _newestTouchPosition1 = new Vector2(4, 6);
            _input.GetTouchPosition(0).Returns(_lastTouchPosition1, _currentTouchPosition1, _newestTouchPosition1);

            _lastTouchPosition2 = new Vector2(4, 2);
            _currentTouchPosition2 = new Vector2(3, 1);
            _newestTouchPosition2 = new Vector2(2, 0);
            _input.GetTouchPosition(1).Returns(_lastTouchPosition2, _currentTouchPosition2, _newestTouchPosition2);
        }

        [Test]
        public void _updater_Updated_TouchCountNot2()
        {
            _input.TouchCount.Returns(1);
            
            _updater.Updated += Raise.Event();

            Assert.AreEqual(0, _pinchStartCount);
            Assert.AreEqual(0, _pinchEndCount);
        }

        [Test]
        public void _updater_Updated_TouchCount2_PinchStart()
        {
            _input.TouchCount.Returns(2);

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _pinchStartCount);
            Assert.IsNull(_lastPinchEventArgs);
        }

        [Test]
        public void _updater_Updated_TouchCount2_DuringPinch()
        {
            // Start pinch
            _input.TouchCount.Returns(2);
            _updater.Updated += Raise.Event();

            // During pinch 1
            _updater.Updated += Raise.Event();

            float lastDistance = Vector2.Distance(_lastTouchPosition1, _lastTouchPosition2);
            float currentDistance = Vector2.Distance(_currentTouchPosition1, _currentTouchPosition2);
            float delta1 = currentDistance - lastDistance;
            PinchEventArgs firstPinchArgs = new PinchEventArgs(_currentTouchPosition1, delta1);
            Assert.AreEqual(firstPinchArgs, _lastPinchEventArgs);

            // During pinch 2
            _updater.Updated += Raise.Event();

            float newestDistance = Vector2.Distance(_newestTouchPosition1, _newestTouchPosition2);
            float delta2 = newestDistance - currentDistance;
            PinchEventArgs secondPinchArgs = new PinchEventArgs(_newestTouchPosition1, delta2);
            Assert.AreEqual(secondPinchArgs, _lastPinchEventArgs);
        }

        [Test]
        public void _updater_Updated_TouchCount2_PinchEnd()
        {
            // Start pinch
            _input.TouchCount.Returns(2);
            _updater.Updated += Raise.Event();

            // End pinch
            _input.TouchCount.Returns(1);
            
            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _pinchEndCount);
            Assert.IsNull(_lastPinchEventArgs);
        }
    }
}