using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class RocketBarrelController : BarrelController
	{
		private ICircularList<RocketSpawner> _rocketSpawners;
		private Faction _faction;
        private ICruisingProjectileStats _rocketStats;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            RocketSpawner[] rocketSpawners = gameObject.GetComponentsInChildren<RocketSpawner>();
            Assert.IsTrue(rocketSpawners.Length != 0);
            _rocketSpawners = new CircularList<RocketSpawner>(rocketSpawners);
        }

        protected override IProjectileStats GetProjectileStats()
        {
            CruisingProjectileStats rocketStats = GetComponent<CruisingProjectileStats>();
			Assert.IsNotNull(rocketStats);
            _rocketStats = new CruisingProjectileStatsWrapper(rocketStats);
            return _rocketStats;
        }

		public void Initialise(
            ITargetFilter targetFilter, 
            IAngleCalculator angleCalculator, 
            IRotationMovementController rotationMovementController,
			IMovementControllerFactory movementControllerFactory, 
            Faction faction, 
            IFlightPointsProvider flightPointsProvider)
		{
			base.Initialise(targetFilter, angleCalculator, rotationMovementController);

			_faction = faction;

			foreach (RocketSpawner rocketSpawner in _rocketSpawners.Items)
			{
				rocketSpawner.Initialise(_rocketStats, movementControllerFactory, flightPointsProvider);
			}
		}

		protected override void Fire(float angleInDegrees)
		{
			_rocketSpawners.Next().SpawnRocket(
				angleInDegrees,
				transform.IsMirrored(),
				Target,
				_targetFilter,
				_faction);
		}
	}
}
