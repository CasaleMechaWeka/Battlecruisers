using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class ArillteryController : OffensiveTurret
	{
		private ITargetProcessor _targetProcessor;

		protected override void OnInitialised()
		{
			base.OnInitialised();

//			IAngleCalculator angleCalculator = 
//			turretBarrelController.Initialise(Faction);
		}
	}
}
