using BattleCruisers.Targets.TargetFinders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class OffensiveTurret : Turret
	{
		private ITargetFinder _targetFinder;

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Assert.AreEqual(BuildingCategory.Offence, category);
			_targetFinder = _targetFinderFactory.OffensiveBuildingTargetFinder;
		}

		// FELIX  Handle when target is destroyed (ie, when target is not the enemy cruiser)
		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Target = _targetFinder.FindTarget();
		}
	}
}
