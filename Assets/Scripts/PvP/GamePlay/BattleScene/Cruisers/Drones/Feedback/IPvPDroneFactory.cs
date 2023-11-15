using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneCreatedEventArgs : EventArgs
    {
        public IPvPDroneController Drone { get; }

        public PvPDroneCreatedEventArgs(IPvPDroneController drone)
        {
            Assert.IsNotNull(drone);
            Drone = drone;
        }
    }

    public interface IPvPDroneFactory : IPvPPoolableFactory<IPvPDroneController, PvPDroneActivationArgs>
    {
        event EventHandler<PvPDroneCreatedEventArgs> DroneCreated;
    }
}