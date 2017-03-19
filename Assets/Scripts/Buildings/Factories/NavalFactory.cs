using BattleCruisers.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildings.Factories
{
	public class NavalFactory : Factory
	{
		protected override Vector3 FindUnitSpawnPosition(Unit unit)
		{
			float horizontalChange = (Size.x + unit.Size.x) * 0.6f;

			Vector3 direction = transform.right;
			if (_parentCruiser.direction == Direction.Left)
			{
				direction *= -1;
			}

			return transform.position + (direction * horizontalChange);
		}
	}
}