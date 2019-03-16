using System;
using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
    public class XDirectionChangeEventArgs : EventArgs
	{
		public Direction NewDirection { get; }

		public XDirectionChangeEventArgs(Direction newDirection)
		{
			NewDirection = newDirection;
		}
	}

	public interface IMovementController
	{
		Vector2 Velocity { get; set; }

		event EventHandler<XDirectionChangeEventArgs> DirectionChanged;

        void Activate();
		void AdjustVelocity();
	}
}
