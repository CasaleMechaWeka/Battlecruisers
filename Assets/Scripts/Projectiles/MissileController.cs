using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
	public class MissileController : ProjectileController, ITargetProvider
	{
		public  ITarget Target { get; private set; }

		public void Initialise(MissileStats missileStats, Vector2 initialVelocityInMPerS, ITargetFilter targetFilter, ITarget target, 
			IMovementControllerFactory movementControllerFactory, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			base.Initialise(missileStats, initialVelocityInMPerS, targetFilter);

			Target = target;

			_movementController = movementControllerFactory.CreateMissileMovementController(_rigidBody, missileStats.MaxVelocityInMPerS, this, targetPositionPredictorFactory);

			target.Destroyed += Target_Destroyed;
		}

		// FELIX  Don't instantly destroy missile, let it go until some maximum range/time
		private void Target_Destroyed(object sender, DestroyedEventArgs e)
		{
			CleanUp();
		}

		protected override void CleanUp()
		{
			Target.Destroyed -= Target_Destroyed;
			base.CleanUp();
		}
	}
}