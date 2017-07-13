using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
	public class DummyMovementController : IMovementController
	{
		public Vector2 Velocity { get { return default(Vector2); } }
		public void AdjustVelocity() { }
	}
}
