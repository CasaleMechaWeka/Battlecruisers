using BattleCruisers.Effects;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    // FELIX   Test, use
    public class DroneFeedback : IDroneFeedback
    {
        private readonly IDroneConsumerInfo _droneConsumerInfo;
        private readonly IPool<IDroneController, Vector2> _dronePool;
        private readonly ISpawnPositionFinder _spawnPositionFinder;
        private readonly IList<IDroneController> _drones;

        public IDroneConsumer DroneConsumer => _droneConsumerInfo.DroneConsumer;

        public DroneFeedback(IDroneConsumerInfo droneConsumerInfo, IPool<IDroneController, Vector2> dronePool, ISpawnPositionFinder spawnPositionFinder)
        {
            Helper.AssertIsNotNull(droneConsumerInfo, dronePool, spawnPositionFinder);

            _droneConsumerInfo = droneConsumerInfo;
            _dronePool = dronePool;
            _spawnPositionFinder = spawnPositionFinder;
            _drones = new List<IDroneController>();

            _droneConsumerInfo.DroneConsumer.DroneNumChanged += DroneConsumer_DroneNumChanged;
        }

        private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            if (e.NewNumOfDrones == _drones.Count)
            {
                return;
            }

            Assert.IsTrue(e.NewNumOfDrones >= 0, $"It does not make sense to have a negative number of drones: {e.NewNumOfDrones}");

            while (e.NewNumOfDrones < _drones.Count)
            {
                IDroneController droneToRemove = _drones.Last();
                _drones.RemoveAt(_drones.Count - 1);
                droneToRemove.Deactivate();
            }

            while (e.NewNumOfDrones > _drones.Count)
            {
                Vector2 spawnPosition = _spawnPositionFinder.FindSpawnPosition(_droneConsumerInfo);
                IDroneController droneToAdd = _dronePool.GetItem(spawnPosition);
                _drones.Add(droneToAdd);
            }
        }

        public void DisposeManagedState()
        {
            foreach (IDroneController drone in _drones)
            {
                drone.Deactivate();
            }
            _drones.Clear();

            _droneConsumerInfo.DroneConsumer.DroneNumChanged -= DroneConsumer_DroneNumChanged;
        }
    }
}