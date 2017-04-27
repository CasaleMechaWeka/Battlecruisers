using BattleCruisers.Buildables.Units.Detectors;
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

		public RangedTargetFinder targetFinder;

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Assert.AreEqual(BuildingCategory.Defence, category);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			targetFinder.Initialise(Faction, turretBarrelController.turretStats.rangeInM);
			targetFinder.TargetFound += TargetFinder_TargetFound;

			Target = targetFinder.FindTarget();
		}

		private void TargetFinder_TargetFound(object sender, EventArgs e)
		{
			if (Target == null)
			{
				Target = targetFinder.FindTarget();
				Target.Destroyed += Enemy_Destroyed;
			}
		}

		private void Enemy_Destroyed(object sender, EventArgs e)
		{
			Assert.AreEqual(Target, sender);
			
			Target.Destroyed -= Enemy_Destroyed;
			Target = targetFinder.FindTarget();
		}
	}
}
