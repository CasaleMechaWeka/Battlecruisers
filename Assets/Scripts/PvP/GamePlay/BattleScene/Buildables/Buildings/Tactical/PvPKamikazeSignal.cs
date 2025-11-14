using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPKamikazeSignal : PvPBuilding
    {
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalUltrasProviders);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders);
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                GameObject[] aircraftGameObjects = GameObject.FindGameObjectsWithTag(GameObjectTags.AIRCRAFT);

                foreach (GameObject aircraftGameObject in aircraftGameObjects)
                {
                    PvPAircraftController aircraftController = aircraftGameObject.GetComponentInChildren<PvPAircraftController>();
                    Assert.IsNotNull(aircraftController);

                    if (aircraftController.BuildableState == PvPBuildableState.Completed
                        && !aircraftController.IsDestroyed)
                    {
                        aircraftController.Kamikaze(EnemyCruiser);
                    }
                }

                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
            }

        }
    }
}
