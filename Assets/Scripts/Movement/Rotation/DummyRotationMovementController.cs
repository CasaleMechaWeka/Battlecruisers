namespace BattleCruisers.Movement.Rotation
{
	public class DummyRotationMovementController : IRotationMovementController
	{
		private readonly bool _isOnTarget;

		public DummyRotationMovementController(bool isOnTarget = true)
		{
			_isOnTarget = isOnTarget;
		}

		public bool IsOnTarget(float desiredAngleInDegrees)
		{
			return _isOnTarget;
		}

		public void AdjustRotation(float desiredAngleInDegrees) { }
	}
}