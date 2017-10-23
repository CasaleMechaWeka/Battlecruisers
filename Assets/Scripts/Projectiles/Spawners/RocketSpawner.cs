using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public class RocketSpawner : ProjectileSpawner
	{
        private ICruisingProjectileStats _rocketStats;
		private IMovementControllerFactory _movementControllerFactory;
		private IFlightPointsProvider _flightPointsProvider;

        public RocketController rocketPrefab;
        protected override ProjectileController ProjectilePrefab { get { return rocketPrefab; } }

        public void Initialise(ICruisingProjectileStats rocketStats, IMovementControllerFactory movementControllerFactory, IFlightPointsProvider flightPointsProvider)
		{
            base.Initialise(rocketStats);

            Helper.AssertIsNotNull(movementControllerFactory, flightPointsProvider);

			_movementControllerFactory = movementControllerFactory;
			_flightPointsProvider = flightPointsProvider;

            Assert.IsNotNull(rocketStats);
            _rocketStats = rocketStats;
		}

		public void SpawnRocket(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter, Faction faction)
		{
            RocketController rocket = Instantiate(rocketPrefab, transform.position, new Quaternion());
			Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _rocketStats.InitialVelocityInMPerS);
			rocket.Initialise(_rocketStats, missileVelocity, targetFilter, target, _movementControllerFactory, faction, _flightPointsProvider);
		}
	}
}
