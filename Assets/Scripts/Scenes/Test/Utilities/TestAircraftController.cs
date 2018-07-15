using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Movement.Velocity;
using System;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.Test.Utilities
{
	public class TestAircraftController : AircraftController
	{
		private TargetType _targetType;

		// IList is not picked up by the Unit inspector
		public List<Vector2> patrolPoints;

		public IList<Vector2> PatrolPoints
		{
			get { throw new NotImplementedException(); }
			set
			{
				patrolPoints = new List<Vector2>(value);
			}
		}

		public override TargetType TargetType { get { return _targetType; } }
        protected override ISoundKey EngineSoundKey { get { return SoundKeys.Engines.Gunship; } }

		private bool _useDummyMovementController = false; 
		public bool UseDummyMovementController
		{
			private get { return _useDummyMovementController; }
			set
			{
				_useDummyMovementController = value;

				if (_useDummyMovementController)
				{
					// Create bogus patrol points so PatrollingMovementController is 
					// created correctly in AircraftController base class
					PatrolPoints = new List<Vector2> { new Vector2(0, 0), new Vector2(1, 1) };
				}
			}
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			if (UseDummyMovementController)
			{
                ActiveMovementController = DummyMovementController;
			}

            _spriteChooser = _factoryProvider.SpriteChooserFactory.CreateFighterSpriteChooser(this);
		}

		public void SetTargetType(TargetType targetType)
		{
			_targetType = targetType;
		}

		protected override IList<IPatrolPoint> GetPatrolPoints()
		{
			return BCUtils.Helper.ConvertVectorsToPatrolPoints(patrolPoints);
		}
	}
}
