using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Utilities
{
	public class TestAircraftController : AircraftController
	{
		private TargetType _targetType;

		public override TargetType TargetType { get { return _targetType; } }

		public IList<Vector2> PatrolPoints { private get; set; }

		public void SetTargetType(TargetType targetType)
		{
			_targetType = targetType;
		}

		protected override IList<Vector2> GetPatrolPoints()
		{
			return PatrolPoints;
		}
	}
}
