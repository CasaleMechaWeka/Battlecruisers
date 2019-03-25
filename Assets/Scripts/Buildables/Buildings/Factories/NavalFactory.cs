using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class NavalFactory : Factory
	{
		public LayerMask unitsLayerMask;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.NavalFactory;
        public override UnitCategory UnitCategory => UnitCategory.Naval;
		public override LayerMask UnitLayerMask => unitsLayerMask;

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders);
        }

        protected override IUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return _factoryProvider.SpawnDeciderFactory.CreateNavalSpawnPositionFinder(this);
        }
    }
}