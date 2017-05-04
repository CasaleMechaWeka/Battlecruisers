using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class OffensiveTurret : Turret, ITargetConsumer
	{
		private ITargetProcessor _targetProcessor;

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Assert.AreEqual(BuildingCategory.Offence, category);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_targetProcessor = _targetsFactory.GlobalTargetProcessor;
			_targetProcessor.AddTargetConsumer(this);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			if (BuildableState == BuildableState.Completed)
			{
				_targetProcessor.RemoveTargetConsumer(this);
				_targetProcessor = null;
			}
		}
	}
}
