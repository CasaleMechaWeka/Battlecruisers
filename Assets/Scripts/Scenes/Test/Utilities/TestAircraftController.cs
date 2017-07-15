using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Movement.Velocity;
using System;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
	public class TestAircraftController : AircraftController
	{
		private TargetType _targetType;

		public override TargetType TargetType { get { return _targetType; } }

		private IList<IPatrolPoint> _patrolPoints;
		public IList<Vector2> PatrolPoints
		{ 
			private get { throw new NotImplementedException(); }
			set
			{
				_patrolPoints = BCUtils.Helper.ConvertVectorsToPatrolPoints(value);
			}
		}

		private bool _useDummyMovementController = false; 
		public bool UseDummyMovementController
		{
			private get { return _useDummyMovementController; }
			set
			{
				_useDummyMovementController = value;

				if (_useDummyMovementController)
				{
					// Create bogus patorl points so PatrollingMovementController is 
					// creted correctly in AircraftController base class
					PatrolPoints = new List<Vector2> { new Vector2(0, 0), new Vector2(1, 1) };
				}
			}
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			if (UseDummyMovementController)
			{
				_activeMovementController = _movementControllerFactory.CreateDummyMovementController();
			}
		}

		public void SetTargetType(TargetType targetType)
		{
			_targetType = targetType;
		}

		protected override IList<IPatrolPoint> GetPatrolPoints()
		{
			return _patrolPoints;
		}
	}
}
