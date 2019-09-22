using System;
using BattleCruisers.Effects;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneFactory : IDroneFactory
    {
        private readonly IPrefabFactory _prefabFactory;

        public event EventHandler<DroneCreatedEventArgs> DroneCreated;

        public DroneFactory(IPrefabFactory prefabFactory)
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