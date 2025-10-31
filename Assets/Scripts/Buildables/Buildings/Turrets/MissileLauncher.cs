using BattleCruisers.Data.Static;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class MissileLauncher : OffenseTurret
    {
        // DLC  Have own sound
        protected override bool HasSingleSprite => true;
        public ProjectileType projectileType = ProjectileType.Rocket;
        protected override void AddBuildRateBoostProviders(
        GlobalBoostProviders globalBoostProviders,
        IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.RocketBuildingsProviders);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders);
        }
        protected override ObservableCollection<IBoostProvider> TurretFireRateBoostProviders
        {
            get
            {
                return _cruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders;
            }
        }
    }
}
