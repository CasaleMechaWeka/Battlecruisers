using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// FELIX  Avoid duplicate code with BomberController
namespace BattleCruisers.Buildables.Units.Aircraft
{
	public class DeathstarController : AircraftController, ITargetConsumer
	{
		private ITargetProcessor _targetProcessor;

		public float cruisingAltitudeInM;

		private const float CRUISING_HEIGHT_EQUALITY_MARGIN = 1;

		#region Properties
		public ITarget Target { get; set; }

		// FELIX  Common with bomber
		private bool IsAtCruisingHeight
		{
			get
			{
				return Mathf.Abs(transform.position.y - cruisingAltitudeInM) <= CRUISING_HEIGHT_EQUALITY_MARGIN;
			}
		}
		#endregion Properties

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_attackCapabilities.Add(TargetType.Cruiser);
			_attackCapabilities.Add(TargetType.Buildings);

			// FELIX  Get LaserEmitter and LaserTurretStats
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			// FELIX  Initialise LaserEmitter
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Assert.IsTrue(cruisingAltitudeInM > transform.position.y);

			PatrolPoints = _aircraftProvider.FindDeathstarPatrolPoints(transform.position, cruisingAltitudeInM);
			StartPatrolling();

			_targetProcessor = _targetsFactory.OffensiveBuildableTargetProcessor;
			_targetProcessor.AddTargetConsumer(this);
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			// FELIX  Shoot target if within range :)
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			if (BuildableState == BuildableState.Completed)
			{
				_targetProcessor.RemoveTargetConsumer(this);
				_targetProcessor = null;
			}
		}
	}
}
