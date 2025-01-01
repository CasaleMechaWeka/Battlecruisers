using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneFeedbackFactory : IPvPDroneFeedbackFactory
    {
        private readonly IPvPPool<IPvPDroneController, PvPDroneActivationArgs> _dronePool;
        private readonly IPvPSpawnPositionFinder _spawnPositionFinder;
        private readonly Faction _faction;

        public PvPDroneFeedbackFactory(
            IPvPPool<IPvPDroneController, PvPDroneActivationArgs> dronePool,
            IPvPSpawnPositionFinder spawnPositionFinder,
            Faction faction)
        {
            PvPHelper.AssertIsNotNull(dronePool, spawnPositionFinder, faction);

            _dronePool = dronePool;
            _spawnPositionFinder = spawnPositionFinder;
            _faction = faction;
        }

        public IPvPDroneFeedback CreateFeedback(IPvPDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            Assert.IsNotNull(droneConsumer);
            return
                new PvPDroneFeedback(
                    new PvPDroneConsumerInfo(droneConsumer, position, size),
                    _dronePool,
                    _spawnPositionFinder,
                    _faction);
        }

        public IPvPDroneFeedback CreateDummyFeedback()
        {
            return new PvPDummyDroneFeedback();
        }
    }
}