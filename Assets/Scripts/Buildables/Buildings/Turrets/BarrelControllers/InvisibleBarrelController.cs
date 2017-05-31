using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
	/// <summary>
	/// Do not have a barrel to move.
	/// 
	/// Decide if we are on target by checking if the target is within the firing zone.
	/// 
	/// FELIX  Implement firing zone :P  Maybe?  Hm.
	/// </summary>
	public class InvisibleBarrelController : ShellTurretBarrelController 
	{
		protected override bool IsOnTarget(float desiredAngleInDegrees)
		{
			return true;
		}

		protected override void AdjustBarrel(float desiredAngleInDegrees) { }
	}
}
