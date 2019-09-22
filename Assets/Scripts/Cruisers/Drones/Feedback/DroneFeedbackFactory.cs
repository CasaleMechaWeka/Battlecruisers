using BattleCruisers.Buildables;
using BattleCruisers.Effects.Drones;
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
        private readonly IDroneAudioActivenessDecider _droneAudioActivenessDecider;
        private readonly Faction _faction;

        public DroneFeedbackFactory(
            IPool<IDroneController, DroneActivationArgs> dronePool, 
            ISpawnPositionFinder spawnPositionFinder,
            IDroneAudioActivenessDecider droneAudioActivenessDecider,
            Faction faction)
        {
            Helper.AssertIsNotNull(dronePool, spawnPositionFinder, droneAudioActivenessDecider, faction);

            _dronePool = dronePool;
            _spawnPositionFinder = spawnPositionFinder;
            _droneAudioActivenessDecider = droneAudioActivenessDecider;
            _faction = faction;
        }

        public IDroneFeedback CreateFeedback(IDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            Assert.IsNotNull(droneConsumer);
            return
                new DroneFeedback(
                    new DroneConsumerInfo(droneConsumer, position, size),
                    _dronePool,
                    _spawnPositionFinder,
                    _droneAudioActivenessDecider,
                    _faction);
        }

        public IDroneFeedback CreateDummyFeedback()
        {
            return new DummyDroneFeedback();
        }
    }
}