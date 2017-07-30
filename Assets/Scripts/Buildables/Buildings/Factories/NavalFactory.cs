using BattleCruisers.Buildables.Units;
using UnityEngine;

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

		protected override Vector3 FindUnitSpawnPosition(IUnit unit)
		{
			float horizontalChange = (Size.x * 0.6f) + (unit.Size.x * 0.5f);

			// If the factory is facing left it has been mirrored (rotated
			// around the y-axis by 180*).  So it's right is an unmirrored
			// factory's left :/
			Vector3 direction = transform.right;

			return transform.position + (direction * horizontalChange);
		}
	}
}