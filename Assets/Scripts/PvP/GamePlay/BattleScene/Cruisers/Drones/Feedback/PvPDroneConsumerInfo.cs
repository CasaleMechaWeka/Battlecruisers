using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneConsumerInfo : IPvPDroneConsumerInfo
    {
        public IPvPDroneConsumer DroneConsumer { get; }
        public Vector2 Position { get; }
        public Vector2 Size { get; }

        public PvPDroneConsumerInfo(IPvPDroneConsumer droneConsumer, Vector2 position, Vector2 size)
        {
            DroneConsumer = droneConsumer;
            Position = position;
            Size = size;
        }
    }
}