namespace BattleCruisers.Movement.Rotation
{
	public interface IRotationMovementController
	{
		bool IsOnTarget(float desiredAngleInDegrees);
		void AdjustRotation(float desiredAngleInDegrees);
	}
}