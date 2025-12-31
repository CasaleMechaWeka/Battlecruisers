using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneFactory : IDroneFactory
    {

        public event EventHandler<DroneCreatedEventArgs> DroneCreated;

        public IDroneController CreateItem()
        {
            IDroneController newDrone = PvPPrefabFactory.CreateDrone();
            DroneCreated?.Invoke(this, new DroneCreatedEventArgs(newDrone));
            return newDrone;
        }
    }
}