using BattleCruisers.Buildables;

namespace BattleCruisers.Movement.Velocity
{
	public interface IHomingMovementController
	{
		ITarget Target { set; }
		void AdjustVelocity();
	}
}
