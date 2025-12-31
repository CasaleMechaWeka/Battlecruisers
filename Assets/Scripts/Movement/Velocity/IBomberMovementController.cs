using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
    public interface IBomberMovementController : IMovementController
	{
		Vector2 TargetVelocity { get; set; }
	}
}
