using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
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

namespace BattleCruisers.Buildables.Buildings.Tactical
{
	public class TeslaCoil : Building, ITargetConsumer
	{
		private BasicTurretStats _teslaCoilStats;
		private CircleTargetDetector _rocketDetector;
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;
		private FireIntervalManager _fireIntervalManager;

		public override TargetValue TargetValue { get { return TargetValue.Medium; } }
		public ITarget Target { get; set; }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_teslaCoilStats = gameObject.GetComponent<BasicTurretStats>();
			Assert.IsNotNull(_teslaCoilStats);

			_rocketDetector = gameObject.GetComponentInChildren<CircleTargetDetector>();
			Assert.IsNotNull(_rocketDetector);

			_attackCapabilities.Add(TargetType.Rocket);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_rocketDetector.Initialise(_teslaCoilStats.rangeInM);
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter enemyDetectionFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			_targetFinder = _targetsFactory.CreateRangedTargetFinder(_rocketDetector, enemyDetectionFilter);
			
			ITargetRanker targetRanker = _targetsFactory.CreateEqualTargetRanker();
			_targetProcessor = _targetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
			_targetProcessor.AddTargetConsumer(this);

			_fireIntervalManager = gameObject.AddComponent<FireIntervalManager>();
			_fireIntervalManager.Initialise(_teslaCoilStats);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			// FELIX  Don't insta-destroy rockets with no feedback, draw some cool lighting (somehow) ?
			if (Target != null && _fireIntervalManager.IsIntervalUp())
			{
				Target.TakeDamage(_teslaCoilStats.damage);
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

				_targetFinder.Dispose();
				_targetFinder = null;
			}
		}
	}
}
