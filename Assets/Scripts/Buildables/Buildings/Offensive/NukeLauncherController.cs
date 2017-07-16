using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
	public class NukeLauncherController : Building
	{
		public override TargetValue TargetValue { get { return TargetValue.High; } }

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			// FELIX  Open & launch nuke :D
		}
	}
}
