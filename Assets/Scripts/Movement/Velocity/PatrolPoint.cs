using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{	
	public class PatrolPoint : IPatrolPoint
	{
		public Vector2 Position { get; }
		public bool RemoveOnceReached { get; }
		public Action ActionOnReached { get; }

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

		public override bool Equals(object obj)
		{
			PatrolPoint other = obj as PatrolPoint;
			return other != null
				&& Position == other.Position;
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode();
		}
	}
}