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
		private Unit _lastUnitProduced;

		public LayerMask unitsLayerMask;

		private const float SPAWN_RADIUS_MULTIPLIER = 1.2f;

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

		protected override void Unit_StartedConstruction(object sender, EventArgs e)
		{
			base.Unit_StartedConstruction(sender, e);

			Unit unit = sender as Unit;
			Assert.IsNotNull(unit);
			_lastUnitProduced = unit;
		}

		/// <returns><c>true</c> if the last produced unit is not blocking the spawn point, otherwise <c>false</c>.</returns>
		protected override bool CanSpawnUnit(Unit unit)
		{
			if (_lastUnitProduced != null && !_lastUnitProduced.IsDestroyed)
			{
				Vector3 spawnPositionV3 = FindUnitSpawnPosition(unit);
				Vector2 spawnPositionV2 = new Vector2(spawnPositionV3.x, spawnPositionV3.y);
				float spawnRadius = SPAWN_RADIUS_MULTIPLIER * unit.Size.x;
				Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPositionV2, spawnRadius, unitsLayerMask);

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