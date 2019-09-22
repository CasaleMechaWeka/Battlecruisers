using BattleCruisers.Effects;
using BattleCruisers.Utils.BattleScene.Pools;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneCreatedEventArgs : EventArgs
    {
        public IDroneController Drone { get; }

        public DroneCreatedEventArgs(IDroneController drone)
        {
            Assert.IsNotNull(drone);
            Drone = drone;
        }
    }

    public interface IDroneFactory : IPoolableFactory<IDroneController, Vector2>
    {
        event EventHandler<DroneCreatedEventArgs> DroneCreated;
    }
}