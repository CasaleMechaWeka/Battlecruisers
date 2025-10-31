using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class NavalFactory : Factory
    {
        public LayerMask unitsLayerMask;

        public override UnitCategory UnitCategory => UnitCategory.Naval;
        public override LayerMask UnitLayerMask => unitsLayerMask;

        protected override IUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return new NavalFactorySpawnPositionFinder(this);
        }
    }
}