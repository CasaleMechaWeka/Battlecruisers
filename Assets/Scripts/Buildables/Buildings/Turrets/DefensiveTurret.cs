using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public abstract class DefensiveTurret : Turret
	{
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;

		public TargetDetector enemyDetector;

		public override void StaticInitialise()
		{
			base.StaticInitialise();
			Assert.AreEqual(BuildingCategory.Defence, category);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			enemyDetector.Initialise(_barrelController.TurretStats.rangeInM);
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			bool isDetectable = true;
			ITargetFilter enemyDetectionFilter = _targetsFactory.CreateDetectableTargetFilter(enemyFaction, isDetectable, _attackCapabilities);
			_targetFinder = _targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

			ITargetRanker targetRanker = _targetsFactory.CreateEqualTargetRanker();
			_targetProcessor = _targetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
			_targetProcessor.AddTargetConsumer(this);
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
