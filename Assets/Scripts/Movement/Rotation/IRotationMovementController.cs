namespace BattleCruisers.Movement
{
	public interface IRotationMovementController
	{
		bool IsOnTarget(float desiredAngleInDegrees);
		void AdjustRotation(float desiredAngleInDegrees);
	}
}