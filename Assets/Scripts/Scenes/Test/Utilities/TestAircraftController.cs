using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class TestAircraftController : AircraftController
	{
		private TargetType _targetType;

		// IList is not picked up by the Unity inspector
		public List<Vector2> patrolPoints;
		public IList<Vector2> PatrolPoints
		{
			get { return patrolPoints; }
			set { patrolPoints = new List<Vector2>(value); }
		}

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Units.Bomber;
        protected override ISoundKey EngineSoundKey => SoundKeys.Engines.Gunship;
		public override TargetType TargetType => _targetType;

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

        public TargetValue targetValue;
        public override TargetValue TargetValue => targetValue;

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
