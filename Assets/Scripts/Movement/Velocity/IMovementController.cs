using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
	public class XDirectionChangeEventArgs : EventArgs
	{
		public Direction NewDirection { get; private set; }

		public XDirectionChangeEventArgs(Direction newDirection)
		{
			NewDirection = newDirection;
		}
	}

	public interface IMovementController
	{
		Vector2 Velocity { get; set; }

		event EventHandler<XDirectionChangeEventArgs> DirectionChanged;

		void AdjustVelocity();
	}
}
