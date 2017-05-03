using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders
{
	/// <summary>
	/// Keeps track of all buildings constructed by the enemy cruiser.
	/// 
	/// When a building reaches 50% completion, it is considered a valid target.
	/// 
	/// This avoids users fooling the AI by starting lots of buildings, but
	/// never committing any resources to them and never completing them.
	/// 
	/// Also has the enemy cruiser as a target.
	/// </summary>
	public class GlobalTargetFinder : ITargetFinder
	{
		private ICruiser _enemyCruiser;

		private const float BUILD_PROGRESS_CONSIDERED_TARGET = 0.5f;

		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

		public GlobalTargetFinder(ICruiser enemyCruiser)
		{
			_enemyCruiser = enemyCruiser;
		}

		public void StartFindingTargets()
		{
			_enemyCruiser.StartedConstruction += EnemyCruiser_StartedConstruction;
			InvokeTargetFoundEvent(_enemyCruiser);
		}

		private void EnemyCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
		{
			IBuildable buildable = e.Buildable;

			buildable.BuildableProgress += Buildable_BuildableProgress;
			buildable.Destroyed += Buildable_Destroyed;
		}

		private void Buildable_BuildableProgress(object sender, BuildProgressEventArgs e)
		{
			if (e.Buildable.BuildProgress >= BUILD_PROGRESS_CONSIDERED_TARGET)
			{
				e.Buildable.BuildableProgress -= Buildable_BuildableProgress;
				InvokeTargetFoundEvent(e.Buildable);
			}
		}

		private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
		{
			e.DestroyedFactionable.Destroyed -= Buildable_Destroyed;

			IBuildable buildable = e.DestroyedFactionable as IBuildable;
			Assert.IsNotNull(buildable);

			if (buildable.BuildProgress >= BUILD_PROGRESS_CONSIDERED_TARGET
				&& TargetLost != null)
			{
				TargetLost.Invoke(this, new TargetEventArgs(buildable));
			}
		}

		private void InvokeTargetFoundEvent(IFactionable target)
		{
			if (TargetFound != null)
			{
				TargetFound.Invoke(this, new TargetEventArgs(target));
			}
		}

		public void Dispose()
		{
			_enemyCruiser.StartedConstruction -= EnemyCruiser_StartedConstruction;
			_enemyCruiser = null;
		}
	}
}
