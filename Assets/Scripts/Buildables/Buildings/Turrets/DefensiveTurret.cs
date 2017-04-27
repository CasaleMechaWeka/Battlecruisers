using BattleCruisers.Buildables.Units.Detectors;
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

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Assert.AreEqual(BuildingCategory.Defence, category);
			
			enemyDetector.Initialise(Helper.GetOppositeFaction(Faction), turretBarrelController.turretStats.rangeInM);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			enemyDetector.OnEntered = OnEnemyEntered;
			enemyDetector.OnExited = OnEnemyExited;
		}

		private void OnEnemyEntered(FactionObject enemy)
		{
			Logging.Log(Tags.DEFENSIVE_TURRET, "OnEnemyEntered()");

			if (_enemy == null)
			{
				_enemy = enemy;
				Target = _enemy.gameObject;

				_enemy.Destroyed += Enemy_Destroyed;
			}
		}

		private void Enemy_Destroyed(object sender, EventArgs e)
		{
			Logging.Log(Tags.DEFENSIVE_TURRET, "Enemy_Destroyed");

			_enemy.Destroyed -= Enemy_Destroyed;
			CleanUpEnemyTarget();
		}

		// FELIX  Switch target to any other enemies that have entered
		private void OnEnemyExited(FactionObject enemy)
		{
			Logging.Log(Tags.DEFENSIVE_TURRET, "OnEnemyExited");

			if (_enemy == enemy)
			{
				CleanUpEnemyTarget();
			}
		}

		private void CleanUpEnemyTarget()
		{
			_enemy = null;
			Target = null;
		}
	}
}
