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
    public class AirFactory : Factory
	{
		public LayerMask aircraftLayerMask;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.AirFactory; } }
        public override UnitCategory UnitCategory { get { return UnitCategory.Aircraft; } }
		public  override LayerMask UnitLayerMask { get { return aircraftLayerMask; } }

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<IObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.AirFactoryBuildRateBoostProviders);
        }

        protected override IUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return new AirFactorySpawnPositionFinder(this);
        }
    }
}