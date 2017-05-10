using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Units.Aircraft
{
	// FELIX  Extract anything common with BomberController to AircraftController?  Eg, shellSpawner?
	public class FighterController : AircraftController, ITargetConsumer
	{
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;
		
		public TargetDetector enemyDetector;
		public BarrelController barrelController;

		public ITarget Target 
		{ 
			get { return barrelController.Target; }
			set 
			{ 
				barrelController.Target = value;

				if (value == null)
				{
					StartPatrolling();
				}
				else
				{
					StopPatrolling();
				}
			}
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Assert.IsNotNull(enemyDetector);
			Assert.IsNotNull(barrelController);

			_attackCapabilities.Add(TargetType.Aircraft);

			// FELIX  Avoid duplicate code with DefensiveTurret.  New class and use composition?
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, TargetType.Aircraft);
			enemyDetector.Initialise(targetFilter, barrelController.turretStats.rangeInM);

			_targetFinder = _targetsFactory.CreateRangedTargetFinder(enemyDetector);
			ITargetRanker targetRanker = _targetsFactory.CreateEqualTargetRanker();
			_targetProcessor = _targetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
			_targetProcessor.AddTargetConsumer(this);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			// FELIX  Handle flying at target & turning around :D
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			// FELIX  Avoid duplicate code with DefensiveTurret.  New class and use composition?
			if (BuildableState == BuildableState.Completed)
			{
				_targetProcessor.RemoveTargetConsumer(this);
				_targetProcessor.Dispose();
				_targetProcessor = null;

				_targetFinder.Dispose();
				_targetFinder = null;
			}
		}
	}
}
