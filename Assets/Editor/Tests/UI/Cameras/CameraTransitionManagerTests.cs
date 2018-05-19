using System.Collections.Generic;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Cameras
{
	public class CameraTransitionManagerTests
    {
		private ICameraTransitionManager _transitionManager;

		private ICamera _camera;
        private ISmoothPositionAdjuster _positionAdjuster;
        private ISmoothZoomAdjuster _zoomAdjuster;
		private ICameraTarget _target1, _target2, _startingState, _invalidTarget;

		private int _cameraTransitionStartedCounter, _cameraTransitionCompletedCounter;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

			_camera = Substitute.For<ICamera>();
			_positionAdjuster = Substitute.For<ISmoothPositionAdjuster>();
			_zoomAdjuster = Substitute.For<ISmoothZoomAdjuster>();

			float target1OrthographicSize = 5;
			_target1
    			= new CameraTarget(
    				new Vector3(-35, 0, -10),
    				target1OrthographicSize,
    				CameraState.PlayerCruiser,
				    CameraState.Overview);

			float target2OrthographicSize = 35;
			_target2
    			= new CameraTarget(
    				new Vector3(35, 15, -10),
    				target2OrthographicSize,
				    CameraState.AiCruiser);

			_startingState
    			= new CameraTarget(
    				default(Vector3),
    				default(float),
				    CameraState.Overview);

			IDictionary<CameraState, ICameraTarget> stateToTarget = new Dictionary<CameraState, ICameraTarget>
			{
				{ _target1.State, _target1 },
				{ _target2.State, _target2 },
				{ _startingState.State, _startingState }
			};

			ICameraTargetsFactory cameraTargetsFactory = Substitute.For<ICameraTargetsFactory>();
			cameraTargetsFactory.CreateCameraTargets().Returns(stateToTarget);

			_transitionManager
    			= new CameraTransitionManager(
        			_camera,
        			cameraTargetsFactory,
        			_positionAdjuster,
    				_zoomAdjuster);

			_cameraTransitionStartedCounter = 0;
			_cameraTransitionCompletedCounter = 0;

			_transitionManager.CameraTransitionStarted += (sender, e) => _cameraTransitionStartedCounter++;
			_transitionManager.CameraTransitionCompleted += (sender, e) => _cameraTransitionCompletedCounter++;

			_invalidTarget = new CameraTarget(default(Vector3), 0, CameraState.InTransition);
        }

		#region SetCameraTarget
		[Test]
        public void SetCameraTarget_DuringTransition_DoesNothing()
        {
			MoveToTransitioningState();

			Assert.IsFalse(_transitionManager.SetCameraTarget(_target1.State));
			Assert.AreEqual(0, _cameraTransitionStartedCounter);
        }

        [Test]
		public void SetCameraTarget_SameAsCurrentState_DoesNothing()
        {
			Assert.IsFalse(_transitionManager.SetCameraTarget(_startingState.State));
			Assert.AreEqual(0, _cameraTransitionStartedCounter);
        }

        [Test]
        public void SetCameraTarget_InvalidTarget_Throws()
        {
			Assert.Throws<UnityAsserts.AssertionException>(() => _transitionManager.SetCameraTarget(_invalidTarget.State));
        }

        [Test]
        public void SetCameraTarget_InstaMove()
        {
			Assert.IsTrue(_transitionManager.SetCameraTarget(_target1.State));

			Assert.AreEqual(_target1.Position, _camera.Position);
			Assert.AreEqual(_target1.OrthographicSize, _camera.OrthographicSize);
			Assert.AreEqual(1, _cameraTransitionStartedCounter);
        }

        [Test]
        public void SetCameraTarget_SmoothMove()
        {
			Assert.IsTrue(_transitionManager.SetCameraTarget(_target2.State));

			Assert.AreEqual(1, _cameraTransitionStartedCounter);
			Assert.AreEqual(CameraState.InTransition, _transitionManager.State);
        }
		#endregion SetCameraTarget

		#region MoveCamera
        [Test]
		public void MoveCamera_TargetNull_DoesNothing()
		{
			_transitionManager.MoveCamera();
			_positionAdjuster.DidNotReceiveWithAnyArgs().AdjustPosition(default(Vector3));
		}

		[Test]
        public void MoveCamera_InTargetState_DoesNothing()
        {
			_transitionManager.SetCameraTarget(_startingState.State);
			_transitionManager.MoveCamera();
            _positionAdjuster.DidNotReceiveWithAnyArgs().AdjustPosition(default(Vector3));
        }

		[Test]
        public void MoveCamera_NotInTargetState_AdjustsCamera()
        {
			_transitionManager.SetCameraTarget(_target2.State);

			_positionAdjuster.AdjustPosition(_target2.Position).Returns(false);
			_zoomAdjuster.AdjustZoom(_target2.OrthographicSize).Returns(false);

			_transitionManager.MoveCamera();

			_positionAdjuster.Received().AdjustPosition(_target2.Position);
			_zoomAdjuster.Received().AdjustZoom(_target2.OrthographicSize);
			Assert.AreEqual(_cameraTransitionCompletedCounter, 0);
			Assert.AreNotEqual(_target2.State, _transitionManager.State);
        }

        [Test]
        public void MoveCamera_NotInTargetState_ReachedTargetState()
        {
			_transitionManager.SetCameraTarget(_target2.State);

            _positionAdjuster.AdjustPosition(_target2.Position).Returns(true);
            _zoomAdjuster.AdjustZoom(_target2.OrthographicSize).Returns(true);

            _transitionManager.MoveCamera();

            _positionAdjuster.Received().AdjustPosition(_target2.Position);
            _zoomAdjuster.Received().AdjustZoom(_target2.OrthographicSize);
            Assert.AreEqual(_cameraTransitionCompletedCounter, 1);
            Assert.AreEqual(_target2.State, _transitionManager.State);
        }
		#endregion MoveCamera

		private void MoveToTransitioningState()
		{
			SetCameraTarget_SmoothMove();
			_cameraTransitionStartedCounter = 0;
		}
    }
}
