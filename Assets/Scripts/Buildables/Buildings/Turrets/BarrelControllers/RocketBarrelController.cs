using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
	public class RocketBarrelController : TurretBarrelController
	{
		private ICircularList<RocketSpawner> _rocketSpawners;
		private Faction _faction;

		public RocketController rocketPrefab;

		private const float ROCKET_CRUISING_ALTITUDE_IN_M = 25;

		public void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator, 
			IMovementControllerFactory movementControllerFactory, Faction faction)
		{
			base.Initialise(targetFilter, angleCalculator);

			Assert.IsNotNull(rocketPrefab);
			_faction = faction;

			RocketSpawner[] rocketSpawners = gameObject.GetComponentsInChildren<RocketSpawner>();
			Assert.IsTrue(rocketSpawners.Length != 0);
			_rocketSpawners = new CircularList<RocketSpawner>(rocketSpawners);

			RocketStats rocketStats = new RocketStats(rocketPrefab, TurretStats.damage, TurretStats.bulletVelocityInMPerS, ROCKET_CRUISING_ALTITUDE_IN_M);
			foreach (RocketSpawner rocketSpawner in _rocketSpawners.Items)
			{
				rocketSpawner.Initialise(rocketStats, movementControllerFactory);
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

