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
	public interface IRangedTargetFinder : ITargetFinder
	{
		// When a target comes within range.
		event EventHandler<TargetEventArgs> TargetFound;

		// When an existing target is lost, either because it moves out of
		// range or is destroyed.
		event EventHandler<TargetEventArgs> TargetLost;
	}

	/// <summary>
	/// Keeps track of all enemies within range.
	/// </summary>
	/// FELIX  Create tests :/  Make FactionObjectDetector an interface 
	public class RangedTargetFinder : MonoBehaviour, ITargetFinder
	{
		private IList<IFactionable> _inRangeEnemies;

		public FactionObjectDetector enemyDetector;

		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

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
				TargetFound.Invoke(this, new TargetEventArgs(enemy));
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

			if (TargetLost != null)
			{
				TargetLost.Invoke(this, new TargetEventArgs(enemy));
			}

			enemy.Destroyed -= Enemy_Destroyed;
		}

		// FELIX  Choose next target better?  Find target at closest angle to current turret angle?
		public IFactionable FindTarget()
		{
			IFactionable target = null;

			if (_inRangeEnemies.Count != 0)
			{
				target = _inRangeEnemies[0];
			}

			return target;
		}
	}
}
