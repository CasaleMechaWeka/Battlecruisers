using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class OffensiveTurret : Turret
	{
		protected override void OnAwake()
		{
			base.OnAwake();
			Assert.AreEqual(BuildingCategory.Offence, category);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();
			Target = _enemyCruiser.gameObject;
		}
	}
}
