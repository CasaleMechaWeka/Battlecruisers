using BattleCruisers.Buildables;

namespace BattleCruisers.Movement.Velocity
{
	public interface IHomingMovementController : IMovementController
	{
		ITarget Target { set; }
	}
}
