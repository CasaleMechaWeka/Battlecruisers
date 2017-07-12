namespace BattleCruisers.Movement
{
	// FELIX Fix namespace
	public interface IRotationMovementController
	{
		bool IsOnTarget(float desiredAngleInDegrees);
		void AdjustRotation(float desiredAngleInDegrees);
	}
}