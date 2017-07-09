using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Offensive
{
	public abstract class OffensiveTurret : Turret, ITargetConsumer
	{
		private ITargetProcessor _targetProcessor;

		public override TargetValue TargetValue { get { return TargetValue.Medium; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.AreEqual(BuildingCategory.Offence, category);
			_attackCapabilities.Add(TargetType.Buildings);
			_attackCapabilities.Add(TargetType.Cruiser);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_targetProcessor = _targetsFactory.OffensiveBuildableTargetProcessor;
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
