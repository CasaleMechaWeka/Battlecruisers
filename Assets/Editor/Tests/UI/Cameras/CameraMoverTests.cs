using BattleCruisers.UI.Cameras;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Cameras
{
	public class DummyCameraMover : CameraMover
	{
		public void SetState(CameraState state)
		{
			State = state;
		}

		public override void MoveCamera(float deltaTime) { }
	}

	public class CameraMoverTests
    {
		private DummyCameraMover _mover;

        [SetUp]
        public void SetuUp()
        {
			_mover = new DummyCameraMover();
        }

        [Test]
        public void InitialState()
        {
			Assert.AreEqual(CameraState.UserInputControlled, _mover.State);
        }

        [Test]
        public void ChangingState_UpdatesProperty_BeforeEmittingEvent()
        {
			CameraState newState = CameraState.LeftMid;

			_mover.StateChanged += (sender, e) => 
			{
				Assert.AreEqual(newState, e.NewState);
				Assert.AreEqual(newState, _mover.State);
			};

			_mover.SetState(newState);
        }

        [Test]
        public void ChangingState_ToSameState_DoesNotEmitEvent()
		{
			_mover.StateChanged += (sender, e) => Assert.Fail();

			_mover.SetState(_mover.State);
		}

        [Test]
        public void Reset_SetsState_AndDoesNotEmitEvent()
        {
			_mover.StateChanged += (sender, e) => Assert.Fail();
            _mover.Reset(CameraState.RightMid);
            Assert.AreEqual(CameraState.RightMid, _mover.State);
        }
    }
}
