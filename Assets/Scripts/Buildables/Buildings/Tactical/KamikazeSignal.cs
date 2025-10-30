using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class KamikazeSignal : Building
    {

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
            base.OnBuildableCompleted();

            GameObject[] aircraftGameObjects = GameObject.FindGameObjectsWithTag(GameObjectTags.AIRCRAFT);

            foreach (GameObject aircraftGameObject in aircraftGameObjects)
            {
                AircraftController aircraftController = aircraftGameObject.GetComponentInChildren<AircraftController>();
                Assert.IsNotNull(aircraftController);

                if (aircraftController.BuildableState == BuildableState.Completed
                    && !aircraftController.IsDestroyed)
                {
                    aircraftController.Kamikaze(EnemyCruiser);
                }
            }
        }
    }
}
