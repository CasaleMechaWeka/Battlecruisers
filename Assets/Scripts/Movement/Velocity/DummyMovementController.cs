using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
	public class DummyMovementController : IMovementController
	{
		public Vector2 Velocity { get; set; } 

		#pragma warning disable 67  // Unused event
		public event EventHandler<XDirectionChangeEventArgs> DirectionChanged;
		#pragma warning restore 67 

		public void AdjustVelocity() { }
	}
}
