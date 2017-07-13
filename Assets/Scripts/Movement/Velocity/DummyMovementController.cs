using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
	public class DummyMovementController : IMovementController
	{
		public Vector2 Velocity { get; set; } 

		public event EventHandler<XDirectionChangeEventArgs> DirectionChanged { add {} remove {} }

		public void AdjustVelocity() { }
	}
}
