using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class Ultralisk : DroneStation
    {
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Ultra; } }

        protected override IObservableCollection<IBoostProvider> BuildRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.UltrasBuildRateBoostProviders;
            }
        }
    }
}
