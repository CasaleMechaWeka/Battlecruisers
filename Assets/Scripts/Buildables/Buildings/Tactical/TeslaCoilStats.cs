using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
	public class TeslaCoilStats : MonoBehaviour
	{
		public float fireRatePerS;
		public float damage;
		public float rangeInM;

		public virtual void Initialise()
		{
			Assert.IsTrue(fireRatePerS > 0);
			Assert.IsTrue(damage > 0);
			Assert.IsTrue(rangeInM > 0);
		}
	}
}

