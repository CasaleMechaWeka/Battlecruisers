using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneFeedback : IPvPDroneFeedback
    {
        private readonly IPvPDroneConsumerInfo _droneConsumerInfo;
        private readonly IPvPPool<IPvPDroneController, PvPDroneActivationArgs> _dronePool;
        private readonly IPvPSpawnPositionFinder _spawnPositionFinder;
        private readonly PvPFaction _faction;
        private readonly IList<IPvPDroneController> _drones;

        public IPvPDroneConsumer DroneConsumer => _droneConsumerInfo.DroneConsumer;

        public PvPDroneFeedback(
            IPvPDroneConsumerInfo droneConsumerInfo,
            IPvPPool<IPvPDroneController, PvPDroneActivationArgs> dronePool,
            IPvPSpawnPositionFinder spawnPositionFinder,
            PvPFaction faction)
        {
            PvPHelper.AssertIsNotNull(droneConsumerInfo, dronePool, spawnPositionFinder);

            _droneConsumerInfo = droneConsumerInfo;
            _dronePool = dronePool;
            _spawnPositionFinder = spawnPositionFinder;
            _faction = faction;
            _drones = new List<IPvPDroneController>();

            _droneConsumerInfo.DroneConsumer.DroneNumChanged += DroneConsumer_DroneNumChanged;

            AddDronesIfNeeded(_droneConsumerInfo.DroneConsumer.NumOfDrones);
        }

        private void DroneConsumer_DroneNumChanged(object sender, PvPDroneNumChangedEventArgs e)
        {
            Assert.IsTrue(e.NewNumOfDrones >= 0, $"It does not make sense to have a negative number of drones: {e.NewNumOfDrones}");

            if (e.NewNumOfDrones == _drones.Count)
            {
                return;
            }

            AddDronesIfNeeded(e.NewNumOfDrones);
            RemoveDronesIfNeeded(e.NewNumOfDrones);
        }

        private async void AddDronesIfNeeded(int numOfDrones)
        {
            while (numOfDrones > _drones.Count)
            {
                PvPDroneActivationArgs activationArgs
                    = new PvPDroneActivationArgs(
                        position: _spawnPositionFinder.FindSpawnPosition(_droneConsumerInfo),
                        _faction);
                IPvPDroneController droneToAdd = await _dronePool.GetItem(activationArgs);
                _drones.Add(droneToAdd);
            }
        }

        private void RemoveDronesIfNeeded(int numOfDrones)
        {
            while (numOfDrones < _drones.Count)
            {
                IPvPDroneController droneToRemove = _drones.Last();
                _drones.RemoveAt(_drones.Count - 1);
                droneToRemove.Deactivate();
            }
        }

        public void DisposeManagedState()
        {
            foreach (IPvPDroneController drone in _drones)
            {
                drone.Deactivate();
            }
            _drones.Clear();

            _droneConsumerInfo.DroneConsumer.DroneNumChanged -= DroneConsumer_DroneNumChanged;
        }
    }
}