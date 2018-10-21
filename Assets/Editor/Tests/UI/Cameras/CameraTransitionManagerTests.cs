using System.Collections.Generic;
using System.Linq;
using BattleCruisers.UI.BattleScene.Navigation;
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
		private INavigationSettings _navigationSettings;
		private ICameraTarget _instaTransitionTarget, _smoothTransitionTarget, _startingTarget, _invalidTarget;
        private IList<CameraStateChangedArgs> _stateChangedArgs;

		private CameraStateChangedArgs LastArgs
		{
			get { return _stateChangedArgs.LastOrDefault(); }
		}

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

			_camera = Substitute.For<ICamera>();
			_positionAdjuster = Substitute.For<ISmoothPositionAdjuster>();
			_zoomAdjuster = Substitute.For<ISmoothZoomAdjuster>();

			_navigationSettings = Substitute.For<INavigationSettings>();
			_navigationSettings.AreTransitionsEnabled.Returns(true);

			float target1OrthographicSize = 5;
			_instaTransitionTarget
    			= new CameraTarget(
    				new Vector3(-35, 0, -10),
    				target1OrthographicSize,
    				CameraState.PlayerCruiser,
				    CameraState.UserInputControlled);

			float target2OrthographicSize = 35;
			_smoothTransitionTarget
    			= new CameraTarget(
    				new Vector3(35, 15, -10),
    				target2OrthographicSize,
				    CameraState.AiCruiser);

			_startingTarget
    			= new CameraTarget(
    				default(Vector3),
    				default(float),
				    CameraState.UserInputControlled);

			IDictionary<CameraState, ICameraTarget> stateToTarget = new Dictionary<CameraState, ICameraTarget>
			{
				{ _instaTransitionTarget.State, _instaTransitionTarget },
				{ _smoothTransitionTarget.State, _smoothTransitionTarget },
				{ _startingTarget.State, _startingTarget }
			};

			ICameraTargetsFactory cameraTargetsFactory = Substitute.For<ICameraTargetsFactory>();
			cameraTargetsFactory.CreateCameraTargets().Returns(stateToTarget);

			_transitionManager
    			= new CameraTransitionManager(
        			_camera,
        			cameraTargetsFactory,
        			_positionAdjuster,
    				_zoomAdjuster,
			        _navigationSettings);

			_stateChangedArgs = new List<CameraStateChangedArgs>();
			_transitionManager.StateChanged += (sender, e) => _stateChangedArgs.Add(e);

			_invalidTarget = new CameraTarget(default(Vector3), 0, CameraState.InTransition);
        }

		#region CameraTarget
		[Test]
        public void SetCameraTarget()
        {
			_transitionManager.CameraTarget = _instaTransitionTarget.State;
        }

        [Test]
        public void SetCameraTarget_InvalidTarget_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _transitionManager.CameraTarget = _invalidTarget.State);
        }
		#endregion CameraTarget

		#region MoveCamera
        [Test]
		public void MoveCamera_TargetNull_Throws()
		{
			Assert.Throws<UnityAsserts.AssertionException>(() => _transitionManager.MoveCamera(default(float)));
		}

		[Test]
        public void MoveCamera_WhileDisabled_DoesNothing()
        {
			_navigationSettings.AreTransitionsEnabled.Returns(false);
			_transitionManager.CameraTarget = _instaTransitionTarget.State;

			_transitionManager.MoveCamera(default(float));

			Assert.AreEqual(_startingTarget.State, _transitionManager.State);
			Assert.IsNull(LastArgs);
        }

		[Test]
        public void MoveCamera_InTargetState_FakeCompletion()
		{
			_transitionManager.CameraTarget = _startingTarget.State;

			_transitionManager.MoveCamera(default(float));

			_positionAdjuster.DidNotReceiveWithAnyArgs().AdjustPosition(default(Vector3));
			AssertFullTransition(_startingTarget.State);
		}

		[Test]
        public void MoveCamera_NotInTargetState_InstaTransition()
        {
			_transitionManager.CameraTarget = _instaTransitionTarget.State;

			_transitionManager.MoveCamera(default(float));

			_positionAdjuster.DidNotReceiveWithAnyArgs().AdjustPosition(default(Vector3));
			Assert.AreEqual(_instaTransitionTarget.Position, _camera.Transform.Position);
			Assert.AreEqual(_instaTransitionTarget.OrthographicSize, _camera.OrthographicSize);
			AssertFullTransition(_instaTransitionTarget.State);
        }

		[Test]
        public void MoveCamera_NotInTargetState_AdjustsCamera()
        {
			_transitionManager.CameraTarget = _smoothTransitionTarget.State;

			_positionAdjuster.AdjustPosition(_smoothTransitionTarget.Position).Returns(false);
			_zoomAdjuster.AdjustZoom(_smoothTransitionTarget.OrthographicSize).Returns(false);

			_transitionManager.MoveCamera(default(float));

			_positionAdjuster.Received().AdjustPosition(_smoothTransitionTarget.Position);
			_zoomAdjuster.Received().AdjustZoom(_smoothTransitionTarget.OrthographicSize);
			Assert.AreEqual(CameraState.InTransition, LastArgs.NewState);
            Assert.AreEqual(CameraState.InTransition, _transitionManager.State);
        }

        [Test]
        public void MoveCamera_NotInTargetState_AdjustsCamera_ReachedTargetState()
        {
			_transitionManager.CameraTarget = _smoothTransitionTarget.State;

            _positionAdjuster.AdjustPosition(_smoothTransitionTarget.Position).Returns(true);
            _zoomAdjuster.AdjustZoom(_smoothTransitionTarget.OrthographicSize).Returns(true);

			_transitionManager.MoveCamera(default(float));

            _positionAdjuster.Received().AdjustPosition(_smoothTransitionTarget.Position);
            _zoomAdjuster.Received().AdjustZoom(_smoothTransitionTarget.OrthographicSize);
			AssertFullTransition(_smoothTransitionTarget.State);
        }

		[Test]
        public void MoveCamera_DuringTransition_DoesNotReachTarget_DoesNotEmitEvent()
        {
			MoveCamera_NotInTargetState_AdjustsCamera();

			_stateChangedArgs.Clear();

            _positionAdjuster.AdjustPosition(_smoothTransitionTarget.Position).Returns(false);
            _zoomAdjuster.AdjustZoom(_smoothTransitionTarget.OrthographicSize).Returns(false);

            _transitionManager.MoveCamera(default(float));

            _positionAdjuster.Received().AdjustPosition(_smoothTransitionTarget.Position);
            _zoomAdjuster.Received().AdjustZoom(_smoothTransitionTarget.OrthographicSize);
			Assert.IsNull(LastArgs);
        }
		#endregion MoveCamera

        private void AssertFullTransition(CameraState targetState)
        {
            Assert.AreEqual(2, _stateChangedArgs.Count);
            Assert.AreEqual(CameraState.InTransition, _stateChangedArgs[0].NewState);
            Assert.AreEqual(targetState, _stateChangedArgs[1].NewState);
			Assert.AreEqual(targetState, _transitionManager.State);
        }
    }
}
