using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class DefensiveTurret : Turret
	{
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;

		public FactionObjectDetector enemyDetector;
		public TargetType targetType;

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Assert.AreEqual(BuildingCategory.Defence, category);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter factionObjectFilter = _targetsFactory.CreateTargetFilter(enemyFaction, targetType);
			enemyDetector.Initialise(factionObjectFilter, turretBarrelController.turretStats.rangeInM);

			_targetFinder = _targetsFactory.CreateRangedTargetFinder(enemyDetector);
			_targetProcessor = _targetsFactory.CreateTargetProcessor(_targetFinder);
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
