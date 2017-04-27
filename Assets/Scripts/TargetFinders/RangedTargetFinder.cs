using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Detectors;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.TargetFinders
{
	/// <summary>
	/// Keeps track of all enemies within range.
	/// </summary>
	/// FELIX  Create tests :/  Make FactionObjectDetector an interface 
	public class RangedTargetFinder : MonoBehaviour, ITargetFinder
	{
		private IList<IFactionable> _inRangeEnemies;

		public FactionObjectDetector enemyDetector;

		public event EventHandler TargetFound;
		
		public bool IsTargetAvailable
		{
			get
			{
				return _inRangeEnemies.Count != 0;
			}
		}

		public void Initialise(Faction faction, float rangeInM)
		{
			_inRangeEnemies = new List<IFactionable>();

			enemyDetector.Initialise(Helper.GetOppositeFaction(faction), rangeInM);

			enemyDetector.OnEntered = OnEnemyEntered;
			enemyDetector.OnExited = OnEnemyExited;
		}

		private void OnEnemyEntered(FactionObject enemy)
		{
			_inRangeEnemies.Add(enemy);

			enemy.Destroyed += Enemy_Destroyed;

			if (TargetFound != null)
			{
				TargetFound.Invoke(this, EventArgs.Empty);
			}
		}

		private void Enemy_Destroyed(object sender, EventArgs e)
		{
			FactionObject enemy = sender as FactionObject;
			Assert.IsNotNull(enemy);
			RemoveEnemy(enemy);
		}

		private void OnEnemyExited(FactionObject enemy)
		{
			RemoveEnemy(enemy);
		}

		private void RemoveEnemy(FactionObject enemy)
		{
			bool didRemoveEnemy = _inRangeEnemies.Remove(enemy);
			Assert.IsTrue(didRemoveEnemy);
		}

		// FELIX  Choose next target better?  Find target at closest angle to current turret angle?
		public IFactionable FindTarget()
		{
			IFactionable target = null;

			if (IsTargetAvailable)
			{
				target = _inRangeEnemies[0];
			}

			return target;
		}
	}
}
