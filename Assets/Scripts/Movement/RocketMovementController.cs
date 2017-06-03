using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Movement
{
	public class RocketMovementController : HomingMovementController
	{
		public RocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
			: base(rigidBody, maxVelocityInMPerS) { }

		protected override Vector2 FindTargetPosition()
		{
			// FELIX
			throw new NotImplementedException();
		}
	}
}

