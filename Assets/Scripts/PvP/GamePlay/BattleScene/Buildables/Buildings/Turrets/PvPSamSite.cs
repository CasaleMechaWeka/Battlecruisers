using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPSamSite : PvPDefenseTurret
    {
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.SamSite;
        protected override SoundKey FiringSound => SoundKeys.Firing.Missile;

        protected override ObservableCollection<IBoostProvider> TurretFireRateBoostProviders
        {
            get
            {
                return _cruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders;
            }
        }
        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> rocketBuildingsBuildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, rocketBuildingsBuildRateBoostProvidersList);
            rocketBuildingsBuildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.RocketBuildingsProviders);
        }
    }
}