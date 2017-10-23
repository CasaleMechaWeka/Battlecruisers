using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class SamSiteBarrelController : BarrelController
	{
		private IExactMatchTargetFilter _exactMatchTargetFilter;
		private MissileSpawner _missileSpawner;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _missileSpawner = gameObject.GetComponentInChildren<MissileSpawner>();
            Assert.IsNotNull(_missileSpawner);        
        }

		public void Initialise(
            IExactMatchTargetFilter targetFilter, 
            IAngleCalculator angleCalculator, 
            IRotationMovementController rotationMovementController,
			IMovementControllerFactory movementControllerFactory, 
            ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			base.Initialise(targetFilter, angleCalculator, rotationMovementController);

			_exactMatchTargetFilter = targetFilter;

            _missileSpawner.Initialise(_projectileStats, movementControllerFactory, targetPositionPredictorFactory);
		}

		protected override void Fire(float angleInDegrees)
		{
			_exactMatchTargetFilter.Target = Target;
			_missileSpawner.SpawnMissile(angleInDegrees, IsSourceMirrored, Target, _exactMatchTargetFilter);
		}
	}
}
