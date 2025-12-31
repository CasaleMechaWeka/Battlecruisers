using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class AirFactory : Factory
    {
        public LayerMask aircraftLayerMask;

        public override UnitCategory UnitCategory => UnitCategory.Aircraft;
        public override LayerMask UnitLayerMask => aircraftLayerMask;

        protected override IUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return new AirFactorySpawnPositionFinder(this);
        }
    }
}