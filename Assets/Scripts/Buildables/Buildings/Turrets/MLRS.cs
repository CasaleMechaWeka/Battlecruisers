using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class MLRS : OffenseTurret
    {
        // DLC  Have own sound
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.RocketLauncher;
        protected override ISoundKey FiringSound => SoundKeys.Firing.RocketLauncher;
        protected override void AddBuildRateBoostProviders(
        IGlobalBoostProviders globalBoostProviders,
        IList<ObservableCollection<IBoostProvider>> rocketBuildingsBuildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, rocketBuildingsBuildRateBoostProvidersList);
            rocketBuildingsBuildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.RocketBuildingsProviders);
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