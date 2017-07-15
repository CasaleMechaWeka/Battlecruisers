using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
	public class DeathstarLauncherController : Building
	{
		public override TargetValue TargetValue { get { return TargetValue.High; } }

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			// FELIX  Launch deathstar :D
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			// FELIX  Destroy deathstar :(
		}
	}
}
