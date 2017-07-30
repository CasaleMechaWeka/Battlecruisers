using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class AirFactory : Factory
	{
		public LayerMask aircraftLayerMask;

		protected override LayerMask UnitLayerMask { get { return aircraftLayerMask; } }

        public override UnitCategory UnitCategory { get { return UnitCategory.Aircraft; } }

		protected override Vector3 FindUnitSpawnPosition(IUnit unit)
		{
			float verticalChange = (Size.y * 0.6f) + (unit.Size.y * 0.5f);
			return transform.position + (transform.up * verticalChange);
		}
	}
}