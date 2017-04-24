using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
	public class AirFactory : Factory
	{
		public LayerMask aircraftLayerMask;

		protected override LayerMask UnitLayerMask
		{
			get
			{
				return aircraftLayerMask;
			}
		}

		protected override Vector3 FindUnitSpawnPosition(Unit unit)
		{
			float verticalChange = (Size.y * 0.6f) + (unit.Size.y * 0.5f);
			return transform.position + (transform.up * verticalChange);
		}
	}
}