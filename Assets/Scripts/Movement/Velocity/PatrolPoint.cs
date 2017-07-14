using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{	
	public class PatrolPoint : IPatrolPoint
	{
		public Vector2 Position { get; private set; }
		public bool RemoveOnceReached { get; private set; }
		public Action ActionOnReached { get; private set; }

		public PatrolPoint(Vector2 position, bool removeOnceReached = false, Action actionOnReached = null)
		{
			Position = position;
			RemoveOnceReached = removeOnceReached;
			ActionOnReached = actionOnReached;

			if (ActionOnReached == null)
			{
				ActionOnReached = () => { };
			}
		}
	}
}