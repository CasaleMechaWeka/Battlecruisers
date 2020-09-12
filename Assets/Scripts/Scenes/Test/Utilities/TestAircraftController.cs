using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Movement.Velocity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
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

        protected async override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			if (UseDummyMovementController)
			{
                ActiveMovementController = DummyMovementController;
			}

            _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateFighterSpriteChooserAsync(this);
		}

		public void SetTargetType(TargetType targetType)
		{
			_targetType = targetType;
		}

		protected override IList<IPatrolPoint> GetPatrolPoints()
		{
			return BCUtils.Helper.ConvertVectorsToPatrolPoints(patrolPoints);
		}

		public void SetHealth(float healthProportion)
		{
			Assert.IsTrue(healthProportion >= 0);
			Assert.IsTrue(healthProportion <= 1);

			float currentProportion = Health / MaxHealth;

			if (Mathf.Approximately(healthProportion, currentProportion))
			{
				return;
			}
			else if (healthProportion > currentProportion)
			{
				float proportionToAdd = healthProportion - currentProportion;
				float healthToAdd = proportionToAdd * MaxHealth;
				RepairCommandExecute(healthToAdd);
			}
			else if(healthProportion < currentProportion)
			{
				float proportionToRemove = currentProportion - healthProportion;
				float healthToRemove = proportionToRemove * MaxHealth;
				TakeDamage(healthToRemove, damageSource: null);
			}
		}
	}
}
