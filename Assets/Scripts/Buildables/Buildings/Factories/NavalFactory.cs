using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class NavalFactory : Factory
	{
		public LayerMask unitsLayerMask;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.NavalFactory; } }
        public override UnitCategory UnitCategory { get { return UnitCategory.Naval; } }
		public override LayerMask UnitLayerMask { get { return unitsLayerMask; } }

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<IObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.NavalFactoryBuildRateBoostProviders);
        }

        protected override IUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return new NavalFactorySpawnPositionFinder(this);
        }
    }
}