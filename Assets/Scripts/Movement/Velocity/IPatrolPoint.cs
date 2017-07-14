using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{	
	public interface IPatrolPoint
	{
		Vector2 Position { get; }
		bool RemoveOnceReached { get; }
		Action ActionOnReached { get; }
	}
}