using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
	public class MissileController : ProjectileController
	{
		private  ITarget _target;

		public void Initialise(MissileStats missileStats, Vector2 initialVelocityInMPerS, ITargetFilter targetFilter, ITarget target, 
			IMovementControllerFactory movementControllerFactory, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			base.Initialise(missileStats, initialVelocityInMPerS, targetFilter);

			_target = target;

			_movementController = movementControllerFactory.CreateMissileMovementController(_rigidBody, missileStats.MaxVelocityInMPerS, targetPositionPredictorFactory);
			_movementController.Target = _target;

			_target.Destroyed += Target_Destroyed;
		}

		// FELIX  Don't instantly destroy missile, let it go until some maximum range/time
		private void Target_Destroyed(object sender, DestroyedEventArgs e)
		{
			CleanUp();
		}

		protected override void CleanUp()
		{
			_movementController.Target = null;
			_target.Destroyed -= Target_Destroyed;

			base.CleanUp();
		}
	}
}