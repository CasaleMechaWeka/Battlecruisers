using BattleCruisers.UI;
using BattleCruisers.UI.Cameras;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Cameras
{
	public class CameraControllerTests
	{
		private CameraController _cameraController;

		private ICameraTransitionManager _transitionManager;
		private ICameraMover _userInputMover;
		private IFilter _shouldNavigationBeEnabledFilter;
		private float _deltaTime;

		[SetUp]
		public void SetuUp()
		{
			_transitionManager = Substitute.For<ICameraTransitionManager>();
			_userInputMover = Substitute.For<ICameraMover>();

			_shouldNavigationBeEnabledFilter = Substitute.For<IFilter>();
			_shouldNavigationBeEnabledFilter.IsMatch.Returns(true);

			_cameraController = new CameraController();
			_cameraController.Initialise(_transitionManager, _userInputMover, _shouldNavigationBeEnabledFilter);

			_deltaTime = 0.1234f;
		}

		[Test]
		public void InitialState()
		{
			_userInputMover.Received().Reset(CameraState.UserInputControlled);
		}

		[Test]
		public void MoveCamera_DefaultMover_IsUserInputMover()
		{
			_cameraController.MoveCamera(_deltaTime);
			_userInputMover.Received().MoveCamera(_deltaTime);
		}

		[Test]
		public void MoveCamera_DisabledDoesNothing()
		{
			_shouldNavigationBeEnabledFilter.IsMatch.Returns(false);
			_cameraController.MoveCamera(_deltaTime);
			_userInputMover.DidNotReceive().MoveCamera(_deltaTime);
		}

		#region Transitions
		[Test]
		public void SwitchToTransitionMover()
		{
			_cameraController.FocusOnPlayerCruiser();
			_transitionManager.Received().Reset(CameraState.UserInputControlled);
			_transitionManager.Received().CameraTarget = CameraState.PlayerCruiser;

			_cameraController.MoveCamera(_deltaTime);
			_transitionManager.Received().MoveCamera(_deltaTime);
		}

		[Test]
		public void FocusOnPlayerCruiser()
		{
			_cameraController.FocusOnPlayerCruiser();
			_transitionManager.Received().CameraTarget = CameraState.PlayerCruiser;
		}

		public void FocusOnAiCruiser()
		{
			_cameraController.FocusOnAiCruiser();
			_transitionManager.Received().CameraTarget = CameraState.AiCruiser;
		}

		public void ShowFullMapView()
		{
			_cameraController.ShowFullMapView();
			_transitionManager.Received().CameraTarget = CameraState.Overview;
		}

		public void ShowMidLeft()
		{
			_cameraController.ShowMidLeft();
			_transitionManager.Received().CameraTarget = CameraState.LeftMid;
		}

		public void ShowMidRight()
		{
			_cameraController.ShowMidRight();
			_transitionManager.Received().CameraTarget = CameraState.RightMid;
		}

		public void DoubleTransition_OnlyDoesFirst()
		{
			_cameraController.ShowMidRight();
			_transitionManager.Received().CameraTarget = CameraState.RightMid;
			_transitionManager.ClearReceivedCalls();

			_cameraController.ShowMidRight();
			_transitionManager.DidNotReceive().CameraTarget = CameraState.RightMid;
		}
		#endregion Transitions

		#region _currentMover_StateChanged
		[Test]
        public void StateChanged_UpdatesState()
		{
			CameraState newState = CameraState.AiCruiser;
            Assert.AreNotEqual(newState, _cameraController.State);

			CameraStateChangedArgs eventArgs = new CameraStateChangedArgs(previousState: CameraState.UserInputControlled, newState: newState);
			_userInputMover.StateChanged += Raise.EventWith(eventArgs);

			Assert.AreEqual(newState, _cameraController.State);
		}

		[Test]
		public void StateChanged_WasInTransition_SwitchesBackToUserInputMover()
		{
			SwitchToTransitionMover();

			CameraStateChangedArgs eventArgs = new CameraStateChangedArgs(previousState: CameraState.InTransition, newState: CameraState.PlayerCruiser);
			_transitionManager.StateChanged += Raise.EventWith(eventArgs);

			_userInputMover.Received().Reset(CameraState.PlayerCruiser);
		}

		[Test]
        public void StateChanged_WasNotInTransition_DoesNotSwitchBackToUserInputMover()
        {
			_userInputMover.ClearReceivedCalls();
            SwitchToTransitionMover();

			CameraStateChangedArgs eventArgs = new CameraStateChangedArgs(previousState: CameraState.Overview, newState: CameraState.InTransition);
            _transitionManager.StateChanged += Raise.EventWith(eventArgs);
		
			_userInputMover.DidNotReceiveWithAnyArgs().Reset(default(CameraState));
        }
		#endregion _currentMover_StateChanged
	}
}
