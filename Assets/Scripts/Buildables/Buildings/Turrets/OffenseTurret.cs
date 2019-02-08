using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils.DataStrctures;

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

        protected override IObservableCollection<IBoostProvider> BuildRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.OffensivesBuildRateBoostProviders;
            }
        }
    }
}