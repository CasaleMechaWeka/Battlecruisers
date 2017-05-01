using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders
{
	/// <summary>
	/// Keeps track of all enemies within range.
	/// </summary>
	public class RangedTargetFinder : MonoBehaviour, ITargetFinder
	{
		private IList<IFactionable> _inRangeEnemies;

		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

		public void Initialise(IFactionObjectDetector enemyDetector)
		{
			_inRangeEnemies = new List<IFactionable>();

			enemyDetector.OnEntered += OnEnemyEntered;
			enemyDetector.OnExited += OnEnemyExited;
		}

		private void OnEnemyEntered(object sender, FactionObjectEventArgs args)
		{
			IFactionable enemy = args.FactionObject;
			_inRangeEnemies.Add(enemy);

			enemy.Destroyed += Enemy_Destroyed;

			if (TargetFound != null)
			{
				TargetFound.Invoke(this, new TargetEventArgs(enemy));
			}
		}

		private void Enemy_Destroyed(object sender, EventArgs e)
		{
			IFactionable enemy = sender as IFactionable;
			Assert.IsNotNull(enemy);
			RemoveEnemy(enemy);
		}

		private void OnEnemyExited(object sender, FactionObjectEventArgs args)
		{
			RemoveEnemy(args.FactionObject);
		}

		private void RemoveEnemy(IFactionable enemy)
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
