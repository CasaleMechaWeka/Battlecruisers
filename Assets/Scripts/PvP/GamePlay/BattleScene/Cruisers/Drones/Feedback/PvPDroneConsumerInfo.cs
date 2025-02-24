using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneConsumerInfo : IDroneConsumerInfo
    {
        public IDroneConsumer DroneConsumer { get; }
        public Vector2 Position { get; }
        public Vector2 Size { get; }

        public PvPDroneConsumerInfo(IDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            DroneConsumer = droneConsumer;
            Position = position;
            Size = size;
        }
    }
}