using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
	public class RocketLauncherController : Building, ITargetConsumer
	{
		private BurstFireTurretStats _rocketLauncherStats;
		private RocketSpawner _rocketSpawner;
		private FireIntervalManager _fireIntervalManager;
		private ITargetFilter _targetFilter;
		private ITargetProcessor _targetProcessor;

		public override TargetValue TargetValue { get { return TargetValue.Medium; } }
		public ITarget Target { get; set; }

		private const float ROCKET_LAUNCH_ANGLE_IN_DEGREES = 90;

		protected override void OnAwake()
		{
			base.OnAwake();

			_rocketLauncherStats = gameObject.GetComponent<BurstFireTurretStats>();
			Assert.IsNotNull(_rocketLauncherStats);

			_rocketSpawner = gameObject.GetComponentInChildren<RocketSpawner>();
			Assert.IsNotNull(_rocketSpawner);

			_attackCapabilities.Add(TargetType.Buildings);
			_attackCapabilities.Add(TargetType.Cruiser);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_fireIntervalManager = gameObject.AddComponent<FireIntervalManager>();
			_fireIntervalManager.Initialise(_rocketLauncherStats);

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			_targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);

			_targetProcessor = _targetsFactory.OffensiveBuildableTargetProcessor;
			_targetProcessor.AddTargetConsumer(this);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (Target != null && _fireIntervalManager.IsIntervalUp())
			{
				_rocketSpawner.SpawnRocket(
					ROCKET_LAUNCH_ANGLE_IN_DEGREES,
					transform.IsMirrored(),
					Target,
					_targetFilter,
					Faction);
			}
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			if (BuildableState == BuildableState.Completed)
			{
				_targetProcessor.RemoveTargetConsumer(this);
				_targetProcessor.Dispose();
				_targetProcessor = null;
			}
		}
	}
}
