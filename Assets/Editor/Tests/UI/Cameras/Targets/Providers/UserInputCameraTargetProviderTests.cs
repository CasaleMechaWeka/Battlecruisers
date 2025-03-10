using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class DummyCameraTargetProvider : UserInputCameraTargetProvider
    {
        public override int Priority => 1;

        public void SetTarget(ICameraTarget target)
        {
            Target = target;
        }

        public void InvokeUserInputEnd()
        {
            UserInputEnd();
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

            _targetChangedCount = 0;
            _cameraTargetProvider.TargetChanged += (sender, e) => _targetChangedCount++;

            _inputStartedCount = 0;
            _cameraTargetProvider.UserInputStarted += (sender, e) => _inputStartedCount++;

            _inputEndedCount = 0;
            _cameraTargetProvider.UserInputEnded += (sender, e) => _inputEndedCount++;
        }

        #region Target
        [Test]
        public void TargetChanged_SameTarget_NoEvent()
        {
            _cameraTargetProvider.SetTarget(null);
            Assert.AreEqual(0, _targetChangedCount);
            Assert.AreEqual(0, _inputStartedCount);
        }

        [Test]
        public void TargetChanged_EquivalentTarget_NoEvent()
        {
            _cameraTargetProvider.SetTarget(_target1);
            _targetChangedCount = 0;
            _inputStartedCount = 0;

            _cameraTargetProvider.SetTarget(_target2);
            Assert.AreEqual(0, _targetChangedCount);
            Assert.AreEqual(0, _inputStartedCount);
        }

        [Test]
        public void TargetChanged_DifferentTarget_Event_FirstTime()
        {
            _cameraTargetProvider.SetTarget(_target1);
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(1, _inputStartedCount);
        }

        [Test]
        public void TargetChanged_DifferentTarget_Event_SecondTime()
        {
            _cameraTargetProvider.SetTarget(_target1);
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(1, _inputStartedCount);

            _cameraTargetProvider.SetTarget(_target3);
            Assert.AreEqual(2, _targetChangedCount);
            Assert.AreEqual(1, _inputStartedCount);

        }
        #endregion Target

        [Test]
        public void UserInputEnd_NotDuringInput()
        {
            _cameraTargetProvider.InvokeUserInputEnd();
            Assert.AreEqual(0, _inputEndedCount);
        }

        [Test]
        public void UserInputEnd_DuringInput()
        {
            // Start user input
            _cameraTargetProvider.SetTarget(_target1);

            _cameraTargetProvider.InvokeUserInputEnd();
            Assert.AreEqual(1, _inputEndedCount);
        }
    }
}