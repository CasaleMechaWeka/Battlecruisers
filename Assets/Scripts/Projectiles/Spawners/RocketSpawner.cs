using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class RocketSpawner : ProjectileSpawner
	{
		private RocketStats _rocketStats;
		private IMovementControllerFactory _movementControllerFactory;
		private IFlightPointsProvider _flightPointsProvider;

		public void Initialise(RocketStats rocketStats, IMovementControllerFactory movementControllerFactory, IFlightPointsProvider flightPointsProvider)
		{
			_rocketStats = rocketStats;
			_movementControllerFactory = movementControllerFactory;
			_flightPointsProvider = flightPointsProvider;
		}

		public void SpawnRocket(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter, Faction faction)
		{
            RocketController rocket = Instantiate(_rocketStats.ProjectilePrefab, transform.position, new Quaternion());
			Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _rocketStats.InitialVelocityInMPerS);
			rocket.Initialise(_rocketStats, missileVelocity, targetFilter, target, _movementControllerFactory, faction, _flightPointsProvider);
		}
	}
}
