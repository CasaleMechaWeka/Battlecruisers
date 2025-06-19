using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneFeedbackFactory
    {
        private readonly IDroneFactory _droneFactory;
        private readonly SpawnPositionFinder _spawnPositionFinder;
        private readonly Faction _faction;

        public DroneFeedbackFactory(
            SpawnPositionFinder spawnPositionFinder,
            IDroneFactory droneFactory,
            Faction faction)
        {
            Helper.AssertIsNotNull(spawnPositionFinder, droneFactory, faction);

            _spawnPositionFinder = spawnPositionFinder;
            _droneFactory = droneFactory;
            _faction = faction;
        }

        public IDroneFeedback CreateFeedback(IDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            Assert.IsNotNull(droneConsumer);
            return
                new DroneFeedback(
                    new DroneConsumerInfo(droneConsumer, position, size),
                    _spawnPositionFinder,
                    _droneFactory,
                    _faction);
        }

        public IDroneFeedback CreateDummyFeedback()
        {
            return new DummyDroneFeedback();
        }
    }
}