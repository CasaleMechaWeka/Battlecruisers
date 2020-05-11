using BattleCruisers.Buildables.Units;
using BattleCruisers.Movement.Velocity.Providers;
using System;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
	public abstract class MovementController : IMovementController
	{
        protected readonly IVelocityProvider _maxVelocityProvider;
        protected readonly ITime _time;

		public abstract Vector2 Velocity { get; set; } 

		public event EventHandler<XDirectionChangeEventArgs> DirectionChanged;

        protected MovementController(IVelocityProvider maxVelocityProvider)
        {
            Assert.IsTrue(maxVelocityProvider.VelocityInMPerS > 0);
            _maxVelocityProvider = maxVelocityProvider;
            _time = TimeBC.Instance;
        }

        public virtual void Activate() { }

        public abstract void AdjustVelocity();

		protected void HandleDirectionChange(Vector2 oldVelocity, Vector2 currentVelocity)
		{
			if (DirectionChanged != null)
			{
				Direction? newDirection = null;

				if (oldVelocity.x >= 0 && currentVelocity.x < 0)
				{
					newDirection = Direction.Left;
				}
				else if (oldVelocity.x <= 0 && currentVelocity.x > 0)
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
