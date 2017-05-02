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
		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

		public void Initialise(IFactionObjectDetector enemyDetector)
		{
			enemyDetector.OnEntered += OnEnemyEntered;
			enemyDetector.OnExited += OnEnemyExited;
		}

		private void OnEnemyEntered(object sender, FactionObjectEventArgs args)
		{
			IFactionable enemy = args.FactionObject;

			enemy.Destroyed += Enemy_Destroyed;

			if (TargetFound != null)
			{
				TargetFound.Invoke(this, new TargetEventArgs(enemy));
			}
		}

		private void Enemy_Destroyed(object sender, DestroyedEventArgs e)
		{
			RemoveEnemy(e.DestroyedFactionable);
		}

		private void OnEnemyExited(object sender, FactionObjectEventArgs args)
		{
			RemoveEnemy(args.FactionObject);
		}

		private void RemoveEnemy(IFactionable enemy)
		{
			if (TargetLost != null)
			{
				TargetLost.Invoke(this, new TargetEventArgs(enemy));
			}

			enemy.Destroyed -= Enemy_Destroyed;
		}
	}
}
