using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Data;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Utils;
using NSubstitute;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects
{
    public class DronesTestGod : NavigationTestGod
    {
        public GameObject parentObject;
        public DroneController dronePrefab;
        public int numOfDrones = 10;
        public float spawnRadiusInM = 0.5f;

        protected override void Setup(Utilities.Helper helper)
        {
            base.Setup(helper);

            // Drones
            for (int i = 0; i < numOfDrones; ++i)
            {
                DroneController newDrone = Instantiate(dronePrefab);
                newDrone.StaticInitialise(helper.CommonStrings);
                newDrone.Activate(
                    new DroneActivationArgs(
                        position: RandomisePosition(parentObject.transform.position),
                        faction: Faction.Blues));
                //Debug.Log($"Created drone #{i} at position: {newDrone.transform.position}");
            }

            // Drones sound
            DroneSoundFeedbackInitialiser droneSoundFeedbackInitialiser = FindObjectOfType<DroneSoundFeedbackInitialiser>();
            Assert.IsNotNull(droneSoundFeedbackInitialiser);

            IBroadcastingProperty<bool> parentCruiserHasActiveDrones = Substitute.For<IBroadcastingProperty<bool>>();
            parentCruiserHasActiveDrones.Value.Returns(true);

            droneSoundFeedbackInitialiser
                .Initialise(
                    parentCruiserHasActiveDrones,
                    ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager);
            parentCruiserHasActiveDrones.ValueChanged += Raise.Event();
        }

        private Vector2 RandomisePosition(Vector2 originalPosition)
        {
            float x = RandomGenerator.Instance.Range(-spawnRadiusInM, spawnRadiusInM);
            float y = RandomGenerator.Instance.Range(-spawnRadiusInM, spawnRadiusInM);
            return new Vector2(originalPosition.x + x, originalPosition.y + y);
        }
    }
}