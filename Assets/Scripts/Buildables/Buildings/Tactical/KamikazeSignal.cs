using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class KamikazeSignal : Building
    {
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Ultra; } }

        protected override IObservableCollection<IBoostProvider> BuildRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.UltrasBuildRateBoostProviders;
            }
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
                    aircraftController.Kamikaze(_enemyCruiser);
				}
            }
        }
    }
}
