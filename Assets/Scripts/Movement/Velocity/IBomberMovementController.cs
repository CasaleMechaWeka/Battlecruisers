using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
	public interface IBomberMovementController : IMovementController
	{
		Vector2 TargetVelocity { get; set; }
	}
}
