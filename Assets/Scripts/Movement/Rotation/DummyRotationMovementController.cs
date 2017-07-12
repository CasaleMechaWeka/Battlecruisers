namespace BattleCruisers.Movement.Rotation
{
	public class DummyRotationMovementController : IRotationMovementController
	{
		public bool IsOnTarget(float desiredAngleInDegrees)
		{
			return true;
		}

		public void AdjustRotation(float desiredAngleInDegrees) { }
	}
}