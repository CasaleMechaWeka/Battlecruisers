using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneFeedbackFactory
    {
        private readonly DroneMonitor _droneMonitor;
        private readonly SpawnPositionFinder _spawnPositionFinder;
        private readonly Faction _faction;

        public DroneFeedbackFactory(
            SpawnPositionFinder spawnPositionFinder,
            DroneMonitor droneMonitor,
            Faction faction)
        {
            Helper.AssertIsNotNull(spawnPositionFinder, droneMonitor, faction);

            _spawnPositionFinder = spawnPositionFinder;
            _droneMonitor = droneMonitor;
            _faction = faction;
        }

        public IDroneFeedback CreateFeedback(IDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            Assert.IsNotNull(droneConsumer);
            return
                new DroneFeedback(
                    new DroneConsumerInfo(droneConsumer, position, size),
                    _spawnPositionFinder,
                    _droneMonitor,
                    _faction);
        }

        public IDroneFeedback CreateDummyFeedback()
        {
            return new DummyDroneFeedback();
        }
    }
}