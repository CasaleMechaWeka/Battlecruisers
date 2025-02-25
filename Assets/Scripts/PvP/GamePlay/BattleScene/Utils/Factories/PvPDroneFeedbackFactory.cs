using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneFeedbackFactory : IDroneFeedbackFactory
    {
        private readonly IPvPPool<IPvPDroneController, DroneActivationArgs> _dronePool;
        private readonly ISpawnPositionFinder _spawnPositionFinder;
        private readonly Faction _faction;

        public PvPDroneFeedbackFactory(
            IPvPPool<IPvPDroneController, DroneActivationArgs> dronePool,
            ISpawnPositionFinder spawnPositionFinder,
            Faction faction)
        {
            PvPHelper.AssertIsNotNull(dronePool, spawnPositionFinder, faction);

            _dronePool = dronePool;
            _spawnPositionFinder = spawnPositionFinder;
            _faction = faction;
        }

        public IDroneFeedback CreateFeedback(IDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            Assert.IsNotNull(droneConsumer);
            return
                new PvPDroneFeedback(
                    new PvPDroneConsumerInfo(droneConsumer, position, size),
                    _dronePool,
                    _spawnPositionFinder,
                    _faction);
        }

        public IDroneFeedback CreateDummyFeedback()
        {
            return new PvPDummyDroneFeedback();
        }
    }
}