using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BattleCruisers.Targets.TargetFinders
{
	public class RangedTargetFinder : ITargetFinder
	{
		private IFactionObjectDetector _enemyDetector;

		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

		public RangedTargetFinder(IFactionObjectDetector enemyDetector)
		{
			_enemyDetector = enemyDetector;

			_enemyDetector.OnEntered += OnEnemyEntered;
			_enemyDetector.OnExited += OnEnemyExited;
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

		public void Dispose()
		{
			_enemyDetector.OnEntered -= OnEnemyEntered;
			_enemyDetector.OnExited -= OnEnemyExited;
			_enemyDetector = null;
		}
	}
}
