using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Detectors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories
{
	public class NavalFactory : Factory
	{
		public LayerMask unitsLayerMask;

		protected override LayerMask UnitLayerMask
		{
			get
			{
				return unitsLayerMask;
			}
		}

		protected override Vector3 FindUnitSpawnPosition(Unit unit)
		{
			float horizontalChange = (Size.x * 0.6f) + (unit.Size.x * 0.5f);

			Vector3 direction = transform.right;
			if (_parentCruiser.Direction == Direction.Left)
			{
				direction *= -1;
			}

			return transform.position + (direction * horizontalChange);
		}
	}
}