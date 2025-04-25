using BattleCruisers.Effects.Drones;
using BattleCruisers.Utils.Fetchers;
using System;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneFactory : IDroneFactory
    {
        public event EventHandler<DroneCreatedEventArgs> DroneCreated;

        public IDroneController CreateItem()
        {
            IDroneController newDrone = PrefabFactory.GetDrone();
            DroneCreated?.Invoke(this, new DroneCreatedEventArgs(newDrone));
            return newDrone;
        }
    }
}