using BattleCruisers.Effects;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneFeedbackFactory : IDroneFeedbackFactory
    {
        private readonly IPool<IDroneController, DroneActivationArgs> _dronePool;
        private readonly ISpawnPositionFinder _spawnPositionFinder;
        private readonly IDroneMonitor _droneMonitor;

        public DroneFeedbackFactory(
            IPool<IDroneController, DroneActivationArgs> dronePool, 
            ISpawnPositionFinder spawnPositionFinder,
            IDroneMonitor droneMonitor)
        {
            Helper.AssertIsNotNull(dronePool, spawnPositionFinder, droneMonitor);

            _dronePool = dronePool;
            _spawnPositionFinder = spawnPositionFinder;
            _droneMonitor = droneMonitor;
        }

        public IDroneFeedback CreateFeedback(IDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            Assert.IsNotNull(droneConsumer);
            return
                new DroneFeedback(
                    new DroneConsumerInfo(droneConsumer, position, size),
                    _dronePool,
                    _spawnPositionFinder,
                    _droneMonitor);
        }

        public IDroneFeedback CreateDummyFeedback()
        {
            return new DummyDroneFeedback();
        }
    }
}