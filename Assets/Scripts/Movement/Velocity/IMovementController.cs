using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
	public interface IMovementController
	{
		Vector2 Velocity { get; }

		void AdjustVelocity();
	}
}
