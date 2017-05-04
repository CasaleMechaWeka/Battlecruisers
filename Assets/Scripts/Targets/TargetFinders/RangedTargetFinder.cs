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
		private ITargetDetector _enemyDetector;

		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

		public RangedTargetFinder(ITargetDetector enemyDetector)
		{
			_enemyDetector = enemyDetector;
		}

		public void StartFindingTargets()
		{
			_enemyDetector.OnEntered += OnEnemyEntered;
			_enemyDetector.OnExited += OnEnemyExited;
		}

		private void OnEnemyEntered(object sender, TargetEventArgs args)
		{
			ITarget enemy = args.Target;

			enemy.Destroyed += Enemy_Destroyed;

			if (TargetFound != null)
			{
				TargetFound.Invoke(this, new TargetEventArgs(enemy));
			}
		}

		private void Enemy_Destroyed(object sender, DestroyedEventArgs e)
		{
			RemoveEnemy(e.DestroyedTarget);
		}

		private void OnEnemyExited(object sender, TargetEventArgs args)
		{
			RemoveEnemy(args.Target);
		}

		private void RemoveEnemy(ITarget enemy)
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
