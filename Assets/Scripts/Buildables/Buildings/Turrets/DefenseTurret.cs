using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public abstract class DefenseTurret : TurretController
    {
        protected override IObservableCollection<IBoostProvider> TurretFireRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.DefenseFireRateBoostProviders;
            }
        }
    }
}