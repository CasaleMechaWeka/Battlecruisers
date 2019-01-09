using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class NavalFactory : Factory
	{
		public LayerMask unitsLayerMask;

		protected override LayerMask UnitLayerMask { get { return unitsLayerMask; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.NavalFactory; } }
        public override UnitCategory UnitCategory { get { return UnitCategory.Naval; } }

        protected override ISpawnPositionFinder CreateSpawnPositionFinder()
        {
            return new NavalFactorySpawnPositionFinder(this);
        }
    }
}