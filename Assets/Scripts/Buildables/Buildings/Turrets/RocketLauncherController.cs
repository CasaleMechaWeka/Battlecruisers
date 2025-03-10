using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class RocketLauncherController : OffenseTurret
    {
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.RocketLauncher;
        protected override ISoundKey FiringSound => SoundKeys.Firing.RocketLauncher;
        public ProjectileType projectileType = ProjectileType.Rocket;

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