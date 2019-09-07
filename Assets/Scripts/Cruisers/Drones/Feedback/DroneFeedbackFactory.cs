using BattleCruisers.Effects;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneFeedbackFactory : IDroneFeedbackFactory
    {
        private readonly IPool<IDroneController, Vector2> _dronePool;
        private readonly ISpawnPositionFinder _spawnPositionFinder;

        public DroneFeedbackFactory(IPool<IDroneController, Vector2> dronePool, ISpawnPositionFinder spawnPositionFinder)
        {
            Helper.AssertIsNotNull(dronePool, spawnPositionFinder);

            _dronePool = dronePool;
            _spawnPositionFinder = spawnPositionFinder;
        }

        public IDroneFeedback CreateFeedback(IDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            Assert.IsNotNull(droneConsumer);
            return
                new DroneFeedback(
                    new DroneConsumerInfo(droneConsumer, position, size),
                    _dronePool,
                    _spawnPositionFinder);
        }

        public IDroneFeedback CreateFeedback(IDroneConsumerInfo droneConsumerInfo)
        {
            Assert.IsNotNull(droneConsumerInfo);
            return new DroneFeedback(droneConsumerInfo, _dronePool, _spawnPositionFinder);
        }
    }
}