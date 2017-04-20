using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
	public class ShieldGenerator : Building
	{
		public ShieldController shieldController;

		protected override void OnAwake()
		{
			base.OnAwake();

			shieldController.gameObject.SetActive(false);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			shieldController.gameObject.SetActive(true);
		}
	}
}
