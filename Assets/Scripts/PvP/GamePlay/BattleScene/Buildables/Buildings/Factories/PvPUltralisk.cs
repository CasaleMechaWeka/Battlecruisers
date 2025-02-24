using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public class PvPUltralisk : PvPDroneStation
    {
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }
    }
}
