using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class MissileBarrelController : BarrelController
	{
        private ICircularList<MissileSpawner> _missileSpawners;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            MissileSpawner[] missileSpawners = gameObject.GetComponentsInChildren<MissileSpawner>();
            Assert.IsTrue(missileSpawners.Length != 0);
            _missileSpawners = new CircularList<MissileSpawner>(missileSpawners);
        }

		public override void Initialise(
            ITargetFilter targetFilter, 
            IAngleCalculator angleCalculator, 
            IRotationMovementController rotationMovementController,
            IFactoryProvider factoryProvider)
		{
            base.Initialise(targetFilter, angleCalculator, rotationMovementController, factoryProvider);

            foreach (MissileSpawner missileSpawner in _missileSpawners.Items)
            {
                missileSpawner.Initialise(_projectileStats, factoryProvider);
			}
		}

		protected override void Fire(float angleInDegrees)
		{
            Logging.Log(Tags.BARREL_CONTROLLER, "MissileBarrelController.Fire()  angleInDegrees: " + angleInDegrees);

            _missileSpawners.Next().SpawnMissile(
                angleInDegrees,
                IsSourceMirrored,
                Target,
                _targetFilter);
		}
	}
}
