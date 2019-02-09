using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public abstract class OffenseTurret : TurretController
    {
        protected override IObservableCollection<IBoostProvider> TurretFireRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.OffenseFireRateBoostProviders;
            }
        }

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<IObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.OffensivesBuildRateBoostProviders);
        }
    }
}