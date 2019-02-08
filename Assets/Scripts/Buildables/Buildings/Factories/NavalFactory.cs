using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class NavalFactory : Factory
	{
		public LayerMask unitsLayerMask;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.NavalFactory; } }
        public override UnitCategory UnitCategory { get { return UnitCategory.Naval; } }
		public override LayerMask UnitLayerMask { get { return unitsLayerMask; } }

        protected override IObservableCollection<IBoostProvider> BuildRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.NavalFactoryBuildRateBoostProviders;
            }
        }

        protected override IUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return new NavalFactorySpawnPositionFinder(this);
        }
    }
}