using BattleCruisers.Buildables;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    // FELIX Update tests :)
    public class DroneFeedback : IDroneFeedback
    {
        private readonly IDroneConsumerInfo _droneConsumerInfo;
        private readonly IPool<IDroneController, DroneActivationArgs> _dronePool;
        private readonly ISpawnPositionFinder _spawnPositionFinder;
        private readonly Faction _faction;
        private readonly IList<IDroneController> _drones;

        public IDroneConsumer DroneConsumer => _droneConsumerInfo.DroneConsumer;

        public DroneFeedback(
            IDroneConsumerInfo droneConsumerInfo, 
            IPool<IDroneController, DroneActivationArgs> dronePool, 
            ISpawnPositionFinder spawnPositionFinder,
            Faction faction)
        {
            Helper.AssertIsNotNull(droneConsumerInfo, dronePool, spawnPositionFinder);

            _droneConsumerInfo = droneConsumerInfo;
            _dronePool = dronePool;
            _spawnPositionFinder = spawnPositionFinder;
            _faction = faction;
            _drones = new List<IDroneController>();

            _droneConsumerInfo.DroneConsumer.DroneNumChanged += DroneConsumer_DroneNumChanged;

            AddDronesIfNeeded(_droneConsumerInfo.DroneConsumer.NumOfDrones);
        }

        private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            Assert.IsTrue(e.NewNumOfDrones >= 0, $"It does not make sense to have a negative number of drones: {e.NewNumOfDrones}");

            if (e.NewNumOfDrones == _drones.Count)
            {
                return;
            }

            AddDronesIfNeeded(e.NewNumOfDrones);
            RemoveDronesIfNeeded(e.NewNumOfDrones);
        }

        private void AddDronesIfNeeded(int numOfDrones)
        {
            while (numOfDrones> _drones.Count)
            {
                DroneActivationArgs activationArgs
                    = new DroneActivationArgs(
                        position: _spawnPositionFinder.FindSpawnPosition(_droneConsumerInfo),
                        _faction);
                IDroneController droneToAdd = _dronePool.GetItem(activationArgs);
                _drones.Add(droneToAdd);
            }
        }

        private void RemoveDronesIfNeeded(int numOfDrones)
        {
            while (numOfDrones < _drones.Count)
            {
                IDroneController droneToRemove = _drones.Last();
                _drones.RemoveAt(_drones.Count - 1);
                droneToRemove.Deactivate();
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