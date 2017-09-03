using BattleCruisers.Buildables.Units;
using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
	public abstract class MovementController : IMovementController
	{
		public abstract Vector2 Velocity { get; set; } 

		public event EventHandler<XDirectionChangeEventArgs> DirectionChanged;

        public virtual void Activate() { }

        public abstract void AdjustVelocity();

		protected void HandleDirectionChange(Vector2 oldVelocity, Vector2 currentVelocity)
		{
			if (DirectionChanged != null)
			{
				Direction? newDirection = null;

				if (oldVelocity.x > 0 && currentVelocity.x < 0)
				{
					newDirection = Direction.Left;
				}
				else if (oldVelocity.x < 0 && currentVelocity.x > 0)
				{
					newDirection = Direction.Right;
				}

				if (newDirection != null)
				{
					DirectionChanged.Invoke(this, new XDirectionChangeEventArgs((Direction)newDirection));
				}
			}
		}
	}
}
