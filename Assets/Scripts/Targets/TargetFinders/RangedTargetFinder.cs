using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

// FELIX  Remove class.  Is dumb wrapper of TargetDetector :P
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
			if (TargetFound != null)
			{
				TargetFound.Invoke(this, args);
			}
		}

		private void OnEnemyExited(object sender, TargetEventArgs args)
		{
			if (TargetLost != null)
			{
				TargetLost.Invoke(this, args);
			}
		}

		public void Dispose()
		{
			_enemyDetector.OnEntered -= OnEnemyEntered;
			_enemyDetector.OnExited -= OnEnemyExited;
			_enemyDetector = null;
		}
	}
}
