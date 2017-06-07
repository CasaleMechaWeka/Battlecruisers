using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units
{
	public class UnitWrapper : BuildableWrapper 
	{
		public Unit Unit { get; private set; }

		public override void Initialise()
		{
			Unit = gameObject.GetComponentInChildren<Unit>();
			Assert.IsNotNull(Unit);
		}
	}
}
