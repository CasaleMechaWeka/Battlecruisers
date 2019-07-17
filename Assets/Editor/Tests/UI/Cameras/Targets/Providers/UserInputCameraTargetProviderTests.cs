using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class DummyCameraTargetProvider : UserInputCameraTargetProvider
    {
        public void SetTarget(ICameraTarget target)
        {
            Target = target;
        }

        public void RaiseInputStarted()
        {
            RaiseUserInputStarted();
        }

        public void RaiseInputEnded()
        {
            RaiseUserInputEnded();
        }
    }

    public class UserInputCameraTargetProviderTests
    {
        private DummyCameraTargetProvider _cameraTargetProvider;
        private ICameraTarget _target1, _target2, _target3;
        private int _targetChangedCount, _inputStartedCount, _inputEndedCount;

        [SetUp]
        public void TestSetup()
        {
            _cameraTargetProvider = new DummyCameraTargetProvider();

            _target1 = new CameraTarget(position: new Vector3(1, 2, 3), orthographicSize: 12);
            _target2 = new CameraTarget(position: new Vector3(1, 2, 3), orthographicSize: 12);
            _target3 = new CameraTarget(position: new Vector3(1, 99, 3), orthographicSize: 12);

            _cameraTargetProvider.SetTarget(_target1);

            _targetChangedCount = 0;
            _cameraTargetProvider.TargetChanged += (sender, e) => _targetChangedCount++;

            _inputStartedCount = 0;
            _cameraTargetProvider.UserInputStarted += (sender, e) => _inputStartedCount++;

            _inputEndedCount = 0;
            _cameraTargetProvider.UserInputEnded += (sender, e) => _inputEndedCount++;
        }

        [Test]
        public void TargetChanged_SameReference_NoEvent()
        {
            _cameraTargetProvider.SetTarget(_target1);
            Assert.AreEqual(0, _targetChangedCount);
        }

        [Test]
        public void TargetChanged_EquivalentTarget_NoEvent()
        {
            _cameraTargetProvider.SetTarget(_target2);
            Assert.AreEqual(0, _targetChangedCount);
        }

        [Test]
        public void TargetChanged_DifferentTarget_Event()
        {
            _cameraTargetProvider.SetTarget(_target3);
            Assert.AreEqual(1, _targetChangedCount);
        }

        [Test]
        public void RaiseUserInputStarted()
        {
            _cameraTargetProvider.RaiseInputStarted();
            Assert.AreEqual(1, _inputStartedCount);
        }

        [Test]
        public void RaiseUserInputEnded()
        {
            _cameraTargetProvider.RaiseInputEnded();
            Assert.AreEqual(1, _inputEndedCount);
        }
    }
}