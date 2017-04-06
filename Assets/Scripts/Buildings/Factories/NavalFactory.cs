using BattleCruisers.Units;
using BattleCruisers.Units.Detectors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildings.Factories
{
	public class NavalFactory : Factory
	{
		private Unit _lastUnitProduced;

		public LayerMask unitsLayerMask;

		private const float SPAWN_SPACE_REQUIRED = 6f;

		protected override Vector3 FindUnitSpawnPosition(Unit unit)
		{
			float horizontalChange = (Size.x * 0.6f) + (unit.Size.x * 0.5f);

			Vector3 direction = transform.right;
			if (_parentCruiser.direction == Direction.Left)
			{
				direction *= -1;
			}

			return transform.position + (direction * horizontalChange);
		}

		protected virtual void Unit_CompletedBuildable(object sender, EventArgs e)
		{
			Unit unit = sender as Unit;
			Assert.IsNotNull(unit);
			_lastUnitProduced = unit;
		}

		/// <returns><c>true</c> if the last produced unit is not blocking the spawn point, otherwise <c>false</c>.</returns>
		protected override bool CanSpawnUnit(Unit unit)
		{
			if (_lastUnitProduced != null && !_lastUnitProduced.IsDestroyed)
			{
				Vector2 position = new Vector2(transform.position.x, transform.position.y);
				Collider2D[] colliders = Physics2D.OverlapCircleAll(position, SPAWN_SPACE_REQUIRED, unitsLayerMask);

				foreach (Collider2D collider in colliders)
				{
					if (collider.gameObject == _lastUnitProduced.gameObject)
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}