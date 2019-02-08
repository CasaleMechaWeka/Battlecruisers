using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    // Excludes SpySatelliteLauncherController, because that inerits from SatelliteLauncherController :)
    public abstract class TacticalBuilding : Building
    {
        protected override IObservableCollection<IBoostProvider> BuildRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.TacticalsBuildRateBoostProviders;
            }
        }
    }
}