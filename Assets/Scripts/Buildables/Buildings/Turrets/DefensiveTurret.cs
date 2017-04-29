using BattleCruisers.TargetFinders;
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
		private FactionObject _enemy;

		public FactionObjectDetector enemyDetector;
		public RangedTargetFinder targetFinder;

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Assert.AreEqual(BuildingCategory.Defence, category);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			enemyDetector.Initialise(Helper.GetOppositeFaction(Faction), turretBarrelController.turretStats.rangeInM);

			targetFinder.Initialise(enemyDetector);
			targetFinder.TargetFound += TargetFinder_TargetFound;
			targetFinder.TargetLost += TargetFinder_TargetLost;

			Target = targetFinder.FindTarget();
		}

		private void TargetFinder_TargetFound(object sender, TargetEventArgs e)
		{
			if (Target == null)
			{
				Target = e.Target;
			}
		}

		private void TargetFinder_TargetLost(object sender, TargetEventArgs e)
		{
			if (Target == e.Target)
			{
				Target = targetFinder.FindTarget();
			}
		}
	}
}
