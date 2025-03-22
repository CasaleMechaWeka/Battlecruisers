using BattleCruisers.Effects.Drones;
using BattleCruisers.Utils.Fetchers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneFactory : IDroneFactory
    {
        private readonly PrefabFactory _prefabFactory;

        public event EventHandler<DroneCreatedEventArgs> DroneCreated;

        public DroneFactory(PrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        public IDroneController CreateItem()
        {
            IDroneController newDrone = _prefabFactory.CreateDrone();
            DroneCreated?.Invoke(this, new DroneCreatedEventArgs(newDrone));
            return newDrone;
        }
    }
}