using BattleCruisers.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneCreatedEventArgs : EventArgs
    {
        public IDroneController Drone { get; }

        public PvPDroneCreatedEventArgs(IDroneController drone)
        {
            Assert.IsNotNull(drone);
            Drone = drone;
        }
    }

    public interface IPvPDroneFactory : IPvPPoolableFactory<IDroneController, DroneActivationArgs>
    {
        event EventHandler<PvPDroneCreatedEventArgs> DroneCreated;
    }
}