using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Units.Aircraft
{
	public class FighterController : AircraftController, ITargetConsumer
	{
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;
		
		public TargetDetector enemyDetector;
		public TurretStats turretStats;
		public AngleCalculator angleCalculator;

		public ITarget Target { private get; set; }


	}
}
