using BattleCruisers.Buildables.Units.Detectors;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class DefensiveTurret : Turret
	{
		public FactionObjectDetector enemyDetector;

		protected override void OnAwake()
		{
			base.OnAwake();

			Assert.AreEqual(BuildingCategory.Defence, category);

			enemyDetector.Radius = turretStats.rangeInM;
			enemyDetector.Initialise(Helper.GetOppositeFaction(Faction));
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			enemyDetector.OnEntered = OnEnemyEntered;
			enemyDetector.OnExited = OnEnemyExited;
		}

		private void OnEnemyEntered(FactionObject enemey)
		{
		}

		private void OnEnemyExited(FactionObject enemey)
		{
		}
	}
}
